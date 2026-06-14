using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Logins;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Accounts
{
    public class LoginService : ILoginRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;
        private static IConfiguration _config;

        public LoginService(AppDbContext dbContext, IMapper mapper, IConfiguration config)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// <summary>
        /// Generate JWT token with user, role, and hospital/branch details
        /// </summary>
        private string GenerateToken(
            string firstName,
            string lastName,
            string middleName,
            string userId,
            string role,
            string hospitalId,
            string branchId,
            string hospitalName,
            string branchName,
            int expireMinutes = 20)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim("FirstName", firstName ?? ""),
                    new Claim("LastName", lastName ?? ""),
                    new Claim("MiddleName", middleName ?? ""),
                    new Claim("UserId", userId),
                    new Claim(ClaimTypes.Role, role ?? ""),
                    new Claim("HospitalId", hospitalId),
                    new Claim("BranchId", branchId),
                    new Claim("HospitalName", hospitalName ?? ""),
                    new Claim("BranchName", branchName ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, $"{firstName} {lastName}".Trim())
                };

                var jwtKey = _config["Jwt:Key"] ?? _config["Jwt:key"];
                if (string.IsNullOrEmpty(jwtKey))
                    throw new Exception("JWT Key is not configured");

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating JWT token: {ex.Message}", ex);
            }
        }

        public async Task<Response> LoginAsyn(LoginDto loginDto)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(loginDto?.UserId) || string.IsNullOrWhiteSpace(loginDto?.Password))
                {
                    var validationError = new
                    {
                        code = "VALIDATION_ERROR",
                        message = "UserId and password are required"
                    };
                    return Response.ErrorResponse(
                        "Validation Failed",
                        validationError,
                        400);
                }

                // Encrypt the input password
                string encryptedPassword = Encrypt(loginDto.Password);

                // Find user by email
                var userData = await _dbContext.UserMaster
                    .Where(u => u.Email.ToUpper() == loginDto.UserId.ToUpper())
                    .Select(u => new
                    {
                        u.Id,
                        u.Email,
                        u.Password,
                        u.IsActive,
                        u.FirstName,
                        u.LastName,
                        u.MiddleName,
                        u.Role,
                        u.HospitalId,
                        u.BranchId,
                        u.ProfileImage
                    })
                    .FirstOrDefaultAsync();

                // Check if user exists
                if (userData == null)
                {
                    var userNotFoundError = new
                    {
                        code = "USER_NOT_FOUND",
                        message = "User with this email does not exist"
                    };
                    return Response.ErrorResponse(
                        "Login Failed",
                        userNotFoundError,
                        401);
                }

                // Verify password (compare encrypted passwords exactly)
                if (userData.Password != encryptedPassword)
                {
                    var passwordError = new
                    {
                        code = "INVALID_PASSWORD",
                        message = "The password you entered is incorrect"
                    };
                    return Response.ErrorResponse(
                        "Login Failed",
                        passwordError,
                        401);
                }

                // Check if user is active
                if (userData.IsActive != true)
                {
                    var inactiveUserError = new
                    {
                        code = "USER_INACTIVE",
                        message = "Your account has been deactivated. Please contact administrator"
                    };
                    return Response.ErrorResponse(
                        "User Account Inactive",
                        inactiveUserError,
                        403);
                }

                // Get hospital details
                var hospitalData = await _dbContext.HospitalMaster
                    .Where(h => h.Id == userData.HospitalId)
                    .Select(h => new { h.HospitalName, h.HospitalLogo })
                    .FirstOrDefaultAsync();

                // Get branch details
                var branchData = await _dbContext.BranchMaster
                    .Where(b => b.Id == userData.BranchId)
                    .Select(b => new { b.BranchName })
                    .FirstOrDefaultAsync();

                // Generate JWT token with hospital and branch details
                string token = GenerateToken(
                    userData.FirstName,
                    userData.LastName,
                    userData.MiddleName ?? "",
                    userData.Id.ToString(),
                    userData.Role ?? "",
                    userData.HospitalId.ToString(),
                    userData.BranchId.ToString(),
                    hospitalData?.HospitalName ?? "Unknown",
                    branchData?.BranchName ?? "Unknown"
                );

                // Create login response with only token (hospital and branch details are in JWT claims)
                var loginData = new
                {
                    token = token
                };

                return Response.Success(
                    loginData,
                    "Login Successful",
                    200);
            }
            catch (Exception ex)
            {
                var errorDetails = new
                {
                    code = "LOGIN_ERROR",
                    message = ex.Message,
                    exceptionType = ex.GetType().Name
                };
                return Response.ErrorResponse(
                    "An error occurred during login",
                    errorDetails,
                    500);
            }
        }

        public (bool IsValid, ClaimsPrincipal? Principal, string? Error) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
                return (true, principal, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        private string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }
    }
}

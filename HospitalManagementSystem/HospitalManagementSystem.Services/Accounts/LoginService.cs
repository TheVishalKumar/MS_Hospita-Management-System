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
        private readonly IConfiguration _config;

        public LoginService(AppDbContext dbContext, IMapper mapper, IConfiguration config)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _config = config;
        }

        private string GenerateToken(
            string firstName,
            string lastName,
            string middleName,
            string id,
            string hospitalId,
            string branchId,
            string hospitalName,
            int expireMinutes = 20)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FirstName", firstName ?? string.Empty),
                new Claim("LastName", lastName ?? string.Empty),
                new Claim("MiddleName", middleName ?? string.Empty),
                new Claim("hospitalId", hospitalId ?? string.Empty),
                new Claim("BranchId", branchId ?? string.Empty),
                new Claim("HospitalName", hospitalName ?? string.Empty)
            };

            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> LoginAsyn(LoginDto loginDto)
        {
            string password = Encrypt(loginDto.Password);

            var dataTask = _dbContext.UserMaster.Join(_dbContext.HospitalMaster,
                UM => UM.HospitalId,
                HM => HM.Id,
                (UM, HM) => new
                {
                    UM.Email,
                    UM.Password,
                    UM.IsActive,
                    UM.FirstName,
                    UM.LastName,
                    UM.MiddleName,
                    UM.Id,
                    UM.HospitalId,
                    UM.BranchId,
                    HM.HospitalName,
                    HM.HospitalLogo,
                    UM.ProfileImage
                }
                )
                .Where(x => x.Email.ToUpper() == loginDto.UserId.ToUpper() && x.Password.ToUpper() == password.ToUpper())
                .FirstOrDefaultAsync();

            var data = await dataTask;

            if (data == null)
            {
                return new Response(Convert.ToInt32(ResponseCode.Failed), CommonMessage.Failed, CommonMessage.LoginFailed);
            }

            if (data.IsActive == false)
            {
                return new Response(Convert.ToInt32(ResponseCode.Failed), CommonMessage.DeactiveUser, CommonMessage.DeactiveUserFailed);
            }

            string token = GenerateToken(
                data.FirstName,
                data.LastName,
                data.MiddleName,
                data.Id.ToString(),
                data.HospitalId.ToString(),
                data.BranchId.ToString(),
                data.HospitalName.ToString());

            // return token in a standard response DTO
            var result = new
            {
                Token = token,
                ExpiresInMinutes = Convert.ToInt32(_config["Jwt:ExpireMinutes"] ?? "20")
            };

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.Success, result);
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

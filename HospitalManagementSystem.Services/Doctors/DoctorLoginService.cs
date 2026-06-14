using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Doctors;
using HospitalManagementSystem.Models.Models.Doctors;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Doctors
{
    public class DoctorLoginService : IDoctorLoginRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public DoctorLoginService(AppDbContext dbContext, IMapper mapper, IConfiguration config)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Doctor login with email or mobile and password
        /// </summary>
        public async Task<Response> LoginAsync(DoctorLoginDto loginDto)
        {
            try
            {
                if (loginDto == null || (string.IsNullOrWhiteSpace(loginDto.EmailId) && string.IsNullOrWhiteSpace(loginDto.MobileNo)))
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Email or Mobile number is required",
                        null);
                }

                if (string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Password is required",
                        null);
                }

                // Find doctor by email or mobile
                var doctor = await _dbContext.DoctorMaster
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d =>
                        (!string.IsNullOrEmpty(loginDto.EmailId) && d.EmailId == loginDto.EmailId) ||
                        (!string.IsNullOrEmpty(loginDto.MobileNo) && d.MobileNo == loginDto.MobileNo));

                if (doctor == null)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor not found",
                        null);
                }

                if (!doctor.IsActive)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor account is inactive",
                        null);
                }

                // Verify password (compare plaintext input against BCrypt hash stored in database)
                // Trim the stored password to handle any whitespace issues
                var storedPassword = doctor.Password?.Trim();
                
                if (string.IsNullOrWhiteSpace(storedPassword))
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor password is not configured",
                        null);
                }

                if (!VerifyPassword(loginDto.Password.Trim(), storedPassword))
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Invalid password",
                        null);
                }

                // Generate JWT token
                var token = GenerateToken(doctor);
                var expiresAt = DateTime.UtcNow.AddMinutes(20);

                var response = new DoctorLoginResponseDto
                {
                    DoctorId = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    MiddleName = doctor.MiddleName,
                    EmailId = doctor.EmailId,
                    MobileNo = doctor.MobileNo,
                    Token = token,
                    ExpiresAt = expiresAt,
                    IsActive = doctor.IsActive
                };

                return new Response(
                    Convert.ToInt32(ResponseCode.Success),
                    "Login successful",
                    response);
            }
            catch (Exception ex)
            {
                return new Response(
                    Convert.ToInt32(ResponseCode.Exception),
                    $"Error during login: {ex.Message}",
                    null);
            }
        }

        /// <summary>
        /// Register a new doctor with password
        /// </summary>
        public async Task<Response> RegisterAsync(CreateDoctorLoginDto createDoctorDto)
        {
            try
            {
                if (createDoctorDto == null)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor data is required",
                        null);
                }

                if (string.IsNullOrWhiteSpace(createDoctorDto.Password) || createDoctorDto.Password.Length < 6)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Password must be at least 6 characters",
                        null);
                }

                // Check if email already exists
                if (!string.IsNullOrEmpty(createDoctorDto.EmailId))
                {
                    var emailExists = await _dbContext.DoctorMaster
                        .AnyAsync(d => d.EmailId == createDoctorDto.EmailId);
                    if (emailExists)
                    {
                        return new Response(
                            Convert.ToInt32(ResponseCode.Failed),
                            "Email already registered",
                            null);
                    }
                }

                // Check if mobile already exists
                if (!string.IsNullOrEmpty(createDoctorDto.MobileNo))
                {
                    var mobileExists = await _dbContext.DoctorMaster
                        .AnyAsync(d => d.MobileNo == createDoctorDto.MobileNo);
                    if (mobileExists)
                    {
                        return new Response(
                            Convert.ToInt32(ResponseCode.Failed),
                            "Mobile number already registered",
                            null);
                    }
                }

                var doctor = _mapper.Map<DoctorMaster>(createDoctorDto);
                doctor.Id = Guid.NewGuid();
                doctor.Password = HashPassword(createDoctorDto.Password);
                doctor.IsActive = true;
                doctor.CreatedDate = DateTime.UtcNow;
                doctor.Version = 0;

                await _dbContext.DoctorMaster.AddAsync(doctor);
                await _dbContext.SaveChangesAsync();

                return new Response(
                    Convert.ToInt32(ResponseCode.Success),
                    "Doctor registered successfully",
                    new { DoctorId = doctor.Id });
            }
            catch (Exception ex)
            {
                return new Response(
                    Convert.ToInt32(ResponseCode.Exception),
                    $"Error during registration: {ex.Message}",
                    null);
            }
        }

        /// <summary>
        /// Get doctor profile by ID
        /// </summary>
        public async Task<DoctorProfileDto> GetDoctorProfileAsync(Guid doctorId)
        {
            var doctor = await _dbContext.DoctorMaster
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == doctorId);

            if (doctor == null)
                return null;

            return _mapper.Map<DoctorProfileDto>(doctor);
        }

        /// <summary>
        /// Update doctor profile
        /// </summary>
        public async Task<Response> UpdateDoctorProfileAsync(UpdateDoctorProfileDto updateDoctorDto)
        {
            try
            {
                var doctor = await _dbContext.DoctorMaster.FirstOrDefaultAsync(d => d.Id == updateDoctorDto.DoctorId);
                if (doctor == null)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor not found",
                        null);
                }

                // Check if new email is not already in use
                if (!string.IsNullOrEmpty(updateDoctorDto.EmailId) && updateDoctorDto.EmailId != doctor.EmailId)
                {
                    var emailExists = await _dbContext.DoctorMaster
                        .AnyAsync(d => d.EmailId == updateDoctorDto.EmailId && d.Id != updateDoctorDto.DoctorId);
                    if (emailExists)
                    {
                        return new Response(
                            Convert.ToInt32(ResponseCode.Failed),
                            "Email already in use",
                            null);
                    }
                }

                // Check if new mobile is not already in use
                if (!string.IsNullOrEmpty(updateDoctorDto.MobileNo) && updateDoctorDto.MobileNo != doctor.MobileNo)
                {
                    var mobileExists = await _dbContext.DoctorMaster
                        .AnyAsync(d => d.MobileNo == updateDoctorDto.MobileNo && d.Id != updateDoctorDto.DoctorId);
                    if (mobileExists)
                    {
                        return new Response(
                            Convert.ToInt32(ResponseCode.Failed),
                            "Mobile number already in use",
                            null);
                    }
                }

                doctor.FirstName = updateDoctorDto.FirstName ?? doctor.FirstName;
                doctor.MiddleName = updateDoctorDto.MiddleName ?? doctor.MiddleName;
                doctor.LastName = updateDoctorDto.LastName ?? doctor.LastName;
                doctor.MobileNo = updateDoctorDto.MobileNo ?? doctor.MobileNo;
                doctor.EmailId = updateDoctorDto.EmailId ?? doctor.EmailId;
                doctor.Address = updateDoctorDto.Address ?? doctor.Address;
                doctor.IsActive = updateDoctorDto.IsActive;
                doctor.UpdateDate = DateTime.UtcNow;
                doctor.UpdateBy = updateDoctorDto.DoctorId;

                _dbContext.DoctorMaster.Update(doctor);
                await _dbContext.SaveChangesAsync();

                return new Response(
                    Convert.ToInt32(ResponseCode.Success),
                    "Doctor profile updated successfully",
                    null);
            }
            catch (Exception ex)
            {
                return new Response(
                    Convert.ToInt32(ResponseCode.Exception),
                    $"Error updating profile: {ex.Message}",
                    null);
            }
        }

        /// <summary>
        /// Change doctor password
        /// </summary>
        public async Task<Response> ChangeDoctorPasswordAsync(ChangeDoctorPasswordDto changePasswordDto)
        {
            try
            {
                if (changePasswordDto == null)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Change password data is required",
                        null);
                }

                var doctor = await _dbContext.DoctorMaster.FirstOrDefaultAsync(d => d.Id == changePasswordDto.DoctorId);
                if (doctor == null)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor not found",
                        null);
                }

                // Verify current password
                if (!VerifyPassword(changePasswordDto.CurrentPassword, doctor.Password))
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Current password is incorrect",
                        null);
                }

                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "New password and confirm password do not match",
                        null);
                }

                if (string.IsNullOrWhiteSpace(changePasswordDto.NewPassword) || changePasswordDto.NewPassword.Length < 6)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Password must be at least 6 characters",
                        null);
                }

                doctor.Password = HashPassword(changePasswordDto.NewPassword);
                doctor.UpdateDate = DateTime.UtcNow;
                doctor.UpdateBy = changePasswordDto.DoctorId;

                _dbContext.DoctorMaster.Update(doctor);
                await _dbContext.SaveChangesAsync();

                return new Response(
                    Convert.ToInt32(ResponseCode.Success),
                    "Password changed successfully",
                    null);
            }
            catch (Exception ex)
            {
                return new Response(
                    Convert.ToInt32(ResponseCode.Exception),
                    $"Error changing password: {ex.Message}",
                    null);
            }
        }

        /// <summary>
        /// Reset doctor password (Admin operation)
        /// </summary>
        public async Task<Response> ResetDoctorPasswordAsync(Guid doctorId, string newPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Password must be at least 6 characters",
                        null);
                }

                var doctor = await _dbContext.DoctorMaster.FirstOrDefaultAsync(d => d.Id == doctorId);
                if (doctor == null)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor not found",
                        null);
                }

                doctor.Password = HashPassword(newPassword);
                doctor.UpdateDate = DateTime.UtcNow;

                _dbContext.DoctorMaster.Update(doctor);
                await _dbContext.SaveChangesAsync();

                return new Response(
                    Convert.ToInt32(ResponseCode.Success),
                    "Password reset successfully",
                    null);
            }
            catch (Exception ex)
            {
                return new Response(
                    Convert.ToInt32(ResponseCode.Exception),
                    $"Error resetting password: {ex.Message}",
                    null);
            }
        }

        /// <summary>
        /// Update doctor password anonymously (no authorization required)
        /// </summary>
        public async Task<Response> UpdateDoctorPasswordAnonymousAsync(UpdateDoctorPasswordAnonymousDto dto)
        {
            try
            {
                if (dto == null || dto.DoctorId == Guid.Empty)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor ID is required",
                        null);
                }

                if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword.Length < 6)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Password must be at least 6 characters",
                        null);
                }

                var doctor = await _dbContext.DoctorMaster.FirstOrDefaultAsync(d => d.Id == dto.DoctorId);
                if (doctor == null)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor not found",
                        null);
                }

                doctor.Password = HashPassword(dto.NewPassword);
                doctor.UpdateDate = DateTime.UtcNow;

                _dbContext.DoctorMaster.Update(doctor);
                await _dbContext.SaveChangesAsync();

                return new Response(
                    Convert.ToInt32(ResponseCode.Success),
                    "Password updated successfully",
                    null);
            }
            catch (Exception ex)
            {
                return new Response(
                    Convert.ToInt32(ResponseCode.Exception),
                    $"Error updating password: {ex.Message}",
                    null);
            }
        }

        /// <summary>
        /// Get all doctors with active status
        /// </summary>
        public async Task<List<DoctorProfileDto>> GetAllDoctorsAsync()
        {
            var doctors = await _dbContext.DoctorMaster
                .AsNoTracking()
                .Where(d => d.IsActive)
                .ToListAsync();

            return _mapper.Map<List<DoctorProfileDto>>(doctors);
        }

        /// <summary>
        /// Set doctor account status
        /// </summary>
        public async Task<Response> SetDoctorStatusAsync(Guid doctorId, bool isActive)
        {
            try
            {
                var doctor = await _dbContext.DoctorMaster.FirstOrDefaultAsync(d => d.Id == doctorId);
                if (doctor == null)
                {
                    return new Response(
                        Convert.ToInt32(ResponseCode.Failed),
                        "Doctor not found",
                        null);
                }

                doctor.IsActive = isActive;
                doctor.UpdateDate = DateTime.UtcNow;

                _dbContext.DoctorMaster.Update(doctor);
                await _dbContext.SaveChangesAsync();

                return new Response(
                    Convert.ToInt32(ResponseCode.Success),
                    $"Doctor account {(isActive ? "activated" : "deactivated")} successfully",
                    null);
            }
            catch (Exception ex)
            {
                return new Response(
                    Convert.ToInt32(ResponseCode.Exception),
                    $"Error updating doctor status: {ex.Message}",
                    null);
            }
        }

        /// <summary>
        /// Check if email exists
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email, Guid? excludeDoctorId = null)
        {
            var query = _dbContext.DoctorMaster.AsQueryable();
            
            if (excludeDoctorId.HasValue)
                query = query.Where(d => d.Id != excludeDoctorId.Value);

            return await query.AnyAsync(d => d.EmailId == email);
        }

        /// <summary>
        /// Check if mobile exists
        /// </summary>
        public async Task<bool> MobileExistsAsync(string mobile, Guid? excludeDoctorId = null)
        {
            var query = _dbContext.DoctorMaster.AsQueryable();
            
            if (excludeDoctorId.HasValue)
                query = query.Where(d => d.Id != excludeDoctorId.Value);

            return await query.AnyAsync(d => d.MobileNo == mobile);
        }

        /// <summary>
        /// Hash password using BCrypt
        /// </summary>
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        /// <summary>
        /// Verify password against hash
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                // Trim both password and hash to remove any whitespace issues
                if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                {
                    System.Diagnostics.Debug.WriteLine($"[VerifyPassword] Null or empty: Password={string.IsNullOrWhiteSpace(password)}, Hash={string.IsNullOrWhiteSpace(hash)}");
                    return false;
                }

                password = password.Trim();
                hash = hash.Trim();

                // Validate BCrypt hash format
                if (!hash.StartsWith("$2a$") && !hash.StartsWith("$2b$") && !hash.StartsWith("$2x$") && !hash.StartsWith("$2y$"))
                {
                    System.Diagnostics.Debug.WriteLine($"[VerifyPassword] Invalid BCrypt hash format. Hash starts with: {hash.Substring(0, Math.Min(10, hash.Length))}");
                    return false;
                }

                // Log the attempt (for debugging)
                System.Diagnostics.Debug.WriteLine($"[VerifyPassword] Attempting verification:");
                System.Diagnostics.Debug.WriteLine($"  Password: {password}");
                System.Diagnostics.Debug.WriteLine($"  Hash: {hash}");
                System.Diagnostics.Debug.WriteLine($"  Hash Length: {hash.Length}");

                // Verify using BCrypt
                bool result = BCrypt.Net.BCrypt.Verify(password, hash);
                System.Diagnostics.Debug.WriteLine($"[VerifyPassword] Result: {result}");
                
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VerifyPassword] Exception: {ex.GetType().Name} - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Generate JWT token for doctor
        /// </summary>
        private string GenerateToken(DoctorMaster doctor)
        {
            var claims = new List<Claim>
            {
                new Claim("FirstName", doctor.FirstName ?? ""),
                new Claim("LastName", doctor.LastName ?? ""),
                new Claim("MiddleName", doctor.MiddleName ?? ""),
                new Claim("DoctorId", doctor.Id.ToString()),
                new Claim("EmailId", doctor.EmailId ?? ""),
                new Claim("MobileNo", doctor.MobileNo ?? ""),
                new Claim(ClaimTypes.NameIdentifier, doctor.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{doctor.FirstName} {doctor.LastName}".Trim())
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
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

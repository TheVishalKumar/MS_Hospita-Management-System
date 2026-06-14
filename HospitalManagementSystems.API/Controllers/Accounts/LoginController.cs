using HospitalManagementSystem.Models.DTO.Logins;
using HospitalManagementSystem.Services.Accounts;
using HospitalManagementSystem.Services.Users;
using HospitalManagementSystem.Services.Security;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystem.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HospitalManagementSystems.API.Controllers.Accounts
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMfaService _mfaService;
        private readonly IUserRepository _userRepository;

        public LoginController(
            ILoginRepository loginRepository, 
            IMfaService mfaService,
            IUserRepository userRepository,
            IConfiguration config)
        {
            _loginRepository = loginRepository;
            _mfaService = mfaService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// User login endpoint - no authentication required
        /// Returns JWT token on successful authentication
        /// </summary>
        /// <param name="loginDto">Login credentials (UserId, Password)</param>
        /// <returns>JWT token if credentials are valid</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ApiResponse<object>> UserLogin([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.UserId) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                var validationError = new
                {
                    code = "VALIDATION_ERROR",
                    message = "UserId and password are required"
                };
                return ApiResponse<object>.Failure(
                    "Validation Failed",
                    new List<string> { "UserId and password are required" });
            }

            try
            {
                var result = await _loginRepository.LoginAsyn(loginDto);
                
                // Check if the Response was successful (StatusCode 200)
                if (result != null && result.StatusCode == 200)
                {
                    // Return success response with the login data
                    return ApiResponse<object>.Success(
                        result.Data, 
                        result.StatusMessage ?? "Login successful");
                }

                // If login failed, return error response with error details
                var errors = new List<string>();
                if (result?.Error != null)
                {
                    errors.Add(result.Error.ToString());
                }
                
                return ApiResponse<object>.Failure(
                    result?.StatusMessage ?? "Login failed",
                    errors);
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Failure(
                    "Login failed",
                    ex.Message);
            }
        }

        /// <summary>
        /// Validate JWT token validity
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("ValidateToken")]
        public ApiResponse<object> ValidateToken([FromBody] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return ApiResponse<object>.Failure(
                    "Invalid token",
                    "Token cannot be empty");
            }

            try
            {
                var result = _loginRepository.ValidateToken(token);
                
                if (result.IsValid)
                {
                    return ApiResponse<object>.Success(
                        new { Valid = true, Claims = result.Principal?.Claims.Select(c => new { c.Type, c.Value }) },
                        "Token is valid");
                }
                else
                {
                    return ApiResponse<object>.Failure(
                        "Invalid token",
                        result.Error ?? "Token validation failed");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Failure(
                    "Token validation error",
                    ex.Message);
            }
        }

        /// <summary>
        /// Setup MFA for Admin account
        /// Only Admin users can set up MFA
        /// Returns QR code and recovery codes
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("SetupMfa")]
        public async Task<ApiResponse<object>> SetupMfa()
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    return ApiResponse<object>.Failure(
                        "Invalid user",
                        "User ID not found in token");
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || user.Role != UserRoles.Admin)
                {
                    return ApiResponse<object>.Failure(
                        "Unauthorized",
                        "Only Admin users can setup MFA");
                }

                // Generate MFA setup
                var mfaSetup = _mfaService.GenerateMfaSecret(user.Email ?? "");

                return ApiResponse<object>.Success(
                    new
                    {
                        Secret = mfaSetup.Secret,
                        QrCodeUrl = mfaSetup.QrCodeUrl,
                        ManualEntryKey = mfaSetup.ManualEntryKey,
                        RecoveryCodes = mfaSetup.RecoveryCodes,
                        Instructions = "1. Scan QR code with authenticator app (Google Authenticator, Microsoft Authenticator, etc.)\n" +
                                      "2. Or manually enter the key above\n" +
                                      "3. Save recovery codes in a secure location\n" +
                                      "4. Use the 6-digit code to confirm setup"
                    },
                    "MFA setup initialized. Complete setup with ConfirmMfaSetup endpoint");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Failure(
                    "MFA setup failed",
                    ex.Message);
            }
        }

        /// <summary>
        /// Confirm MFA setup with 6-digit code from authenticator
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("ConfirmMfaSetup")]
        public async Task<ApiResponse<object>> ConfirmMfaSetup([FromBody] ConfirmMfaDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Secret) || string.IsNullOrWhiteSpace(dto.MfaCode))
            {
                return ApiResponse<object>.Failure(
                    "Invalid input",
                    "Secret and MFA code are required");
            }

            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    return ApiResponse<object>.Failure(
                        "Invalid user",
                        "User ID not found in token");
                }

                // Verify MFA code
                if (!_mfaService.VerifyMfaToken(dto.Secret, dto.MfaCode))
                {
                    return ApiResponse<object>.Failure(
                        "Invalid MFA code",
                        "The 6-digit code is incorrect or expired");
                }

                // Save MFA settings to user
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<object>.Failure(
                        "User not found",
                        "User does not exist");
                }
                user.IsMfaEnabled = true;
                user.MfaSecret = dto.Secret; // Should be encrypted in database
                user.MfaRecoveryCodes = System.Text.Json.JsonSerializer.Serialize(dto.RecoveryCodes ?? new List<string>());
user.UpdateDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                return ApiResponse<object>.Success(
                    new { Enabled = true },
                    "MFA has been successfully enabled for your account");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Failure(
                    "MFA confirmation failed",
                    ex.Message);
            }
        }

        /// <summary>
        /// Disable MFA for user account (requires password confirmation)
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("DisableMfa")]
        public async Task<ApiResponse<object>> DisableMfa([FromBody] DisableMfaDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Password))
            {
                return ApiResponse<object>.Failure(
                    "Invalid input",
                    "Password is required to disable MFA");
            }

            try
            {
                var userIdClaim = User.FindFirst("Id")?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    return ApiResponse<object>.Failure(
                        "Invalid user",
                        "User ID not found in token");
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<object>.Failure(
                        "User not found",
                        "User does not exist");
                }

                // Verify password
                var passwordHasher = new PasswordHashingService();
                if (!passwordHasher.VerifyPassword(dto.Password ?? "", user.Password ?? ""))
                {
                    return ApiResponse<object>.Failure(
                        "Invalid password",
                        "Password is incorrect");
                }

                // Disable MFA
                user.IsMfaEnabled = false;
                user.MfaSecret = null;
                user.MfaRecoveryCodes = null;
                user.UpdateDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                return ApiResponse<object>.Success(
                    new { Enabled = false },
                    "MFA has been successfully disabled");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Failure(
                    "MFA disable failed",
                    ex.Message);
            }
        }

        /// <summary>
        /// Login with MFA code (for users with MFA enabled)
        /// Second step after initial email/password login
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("LoginWithMfa")]
        public async Task<ApiResponse<object>> LoginWithMfa([FromBody] LoginWithMfaDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TempToken) || string.IsNullOrWhiteSpace(dto.MfaCode))
            {
                return ApiResponse<object>.Failure(
                    "Invalid input",
                    "Temporary token and MFA code are required");
            }

            try
            {
                // TODO: Validate temp token and extract user ID
                // Then verify MFA code and return real JWT token

                return ApiResponse<object>.Failure(
                    "Not implemented",
                    "MFA login endpoint requires temp token implementation");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Failure(
                    "MFA login failed",
                    ex.Message);
            }
        }
    }

    /// <summary>
    /// DTO for confirming MFA setup
    /// </summary>
    public class ConfirmMfaDto
    {
        public string? Secret { get; set; }
        public string? MfaCode { get; set; }
        public List<string>? RecoveryCodes { get; set; }
    }

    /// <summary>
    /// DTO for disabling MFA
    /// </summary>
    public class DisableMfaDto
    {
        public string? Password { get; set; }
    }

    /// <summary>
    /// DTO for MFA login
    /// </summary>
    public class LoginWithMfaDto
    {
        public string? TempToken { get; set; }
        public string? MfaCode { get; set; }
    }
}

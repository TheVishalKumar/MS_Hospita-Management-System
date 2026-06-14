using HospitalManagementSystem.Models.DTO.Doctors;
using HospitalManagementSystem.Services.Doctors;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HospitalManagementSystems.API.Controllers.Doctors
{
    /// <summary>
    /// Doctor authentication and profile management endpoints
    /// Provides login, registration, password management, and CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DoctorLoginController : ControllerBase
    {
        private readonly IDoctorLoginRepository _doctorLoginService;

        public DoctorLoginController(IDoctorLoginRepository doctorLoginService)
        {
            _doctorLoginService = doctorLoginService ?? throw new ArgumentNullException(nameof(doctorLoginService));
        }

        /// <summary>
        /// Doctor login with email/mobile and password
        /// </summary>
        /// <param name="loginDto">Email/Mobile and password credentials</param>
        /// <returns>JWT token and doctor profile</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 401)]
        [ProducesResponseType(typeof(Response), 404)]
        public async Task<IActionResult> Login([FromBody] DoctorLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(400, "Invalid request data", null));

            var result = await _doctorLoginService.LoginAsync(loginDto);

            if (result.StatusCode == 1) // Success
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Register a new doctor with login credentials
        /// Only for Admin users
        /// </summary>
        /// <param name="createDoctorDto">Doctor registration data</param>
        /// <returns>Doctor ID on success</returns>
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Response), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 409)]
        public async Task<IActionResult> Register([FromBody] CreateDoctorLoginDto createDoctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(400, "Invalid request data", null));

            var result = await _doctorLoginService.RegisterAsync(createDoctorDto);

            if (result.StatusCode == 1) // Success
                return CreatedAtAction(nameof(GetDoctorProfile), new { doctorId = ((dynamic)result.Data)?.DoctorId }, result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Get doctor profile by ID
        /// </summary>
        /// <param name="doctorId">Doctor ID</param>
        /// <returns>Doctor profile information</returns>
        [HttpGet("{doctorId}")]
        [Authorize]
        [ProducesResponseType(typeof(DoctorProfileDto), 200)]
        [ProducesResponseType(typeof(Response), 404)]
        public async Task<IActionResult> GetDoctorProfile(Guid doctorId)
        {
            if (doctorId == Guid.Empty)
                return BadRequest(new Response(400, "Invalid doctor ID", null));

            var doctor = await _doctorLoginService.GetDoctorProfileAsync(doctorId);
            if (doctor == null)
                return NotFound(new Response(404, "Doctor not found", null));

            return Ok(doctor);
        }

        /// <summary>
        /// Update doctor profile information
        /// </summary>
        /// <param name="updateDoctorDto">Updated doctor data</param>
        /// <returns>Success message</returns>
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateDoctorProfileDto updateDoctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(400, "Invalid request data", null));

            var result = await _doctorLoginService.UpdateDoctorProfileAsync(updateDoctorDto);

            if (result.StatusCode == 1) // Success
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Change doctor password
        /// </summary>
        /// <param name="changePasswordDto">Current password and new password</param>
        /// <returns>Success message</returns>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 401)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeDoctorPasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(400, "Invalid request data", null));

            var result = await _doctorLoginService.ChangeDoctorPasswordAsync(changePasswordDto);

            if (result.StatusCode == 1) // Success
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Reset doctor password (Admin operation)
        /// </summary>
        /// <param name="doctorId">Doctor ID</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Success message</returns>
        [HttpPost("{doctorId}/reset-password")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        public async Task<IActionResult> ResetPassword(Guid doctorId, [FromQuery] string newPassword)
        {
            if (doctorId == Guid.Empty || string.IsNullOrWhiteSpace(newPassword))
                return BadRequest(new Response(400, "Doctor ID and new password are required", null));

            var result = await _doctorLoginService.ResetDoctorPasswordAsync(doctorId, newPassword);

            if (result.StatusCode == 1) // Success
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Update doctor password anonymously (override password by API)
        /// </summary>
        /// <param name="updatePasswordDto">Doctor ID and new password</param>
        /// <returns>Success message</returns>
        [HttpPost("update-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        public async Task<IActionResult> UpdatePasswordAnonymous([FromBody] UpdateDoctorPasswordAnonymousDto updatePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(400, "Invalid request data", null));

            var result = await _doctorLoginService.UpdateDoctorPasswordAnonymousAsync(updatePasswordDto);

            if (result.StatusCode == 1) // Success
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Get all active doctors
        /// </summary>
        /// <returns>List of doctor profiles</returns>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(typeof(System.Collections.Generic.List<DoctorProfileDto>), 200)]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorLoginService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        /// <summary>
        /// Set doctor account status
        /// </summary>
        /// <param name="doctorId">Doctor ID</param>
        /// <param name="isActive">Active status</param>
        /// <returns>Success message</returns>
        [HttpPut("{doctorId}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 404)]
        public async Task<IActionResult> SetDoctorStatus(Guid doctorId, [FromQuery] bool isActive)
        {
            if (doctorId == Guid.Empty)
                return BadRequest(new Response(400, "Invalid doctor ID", null));

            var result = await _doctorLoginService.SetDoctorStatusAsync(doctorId, isActive);

            if (result.StatusCode == 1) // Success
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Check if email is available
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="excludeDoctorId">Doctor ID to exclude from check (for updates)</param>
        /// <returns>Availability status</returns>
        [HttpGet("check-email")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CheckEmailAvailability([FromQuery] string email, [FromQuery] Guid? excludeDoctorId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new Response(400, "Email is required", null));

            var exists = await _doctorLoginService.EmailExistsAsync(email, excludeDoctorId);
            return Ok(new { available = !exists, email });
        }

        /// <summary>
        /// Check if mobile number is available
        /// </summary>
        /// <param name="mobile">Mobile number</param>
        /// <param name="excludeDoctorId">Doctor ID to exclude from check (for updates)</param>
        /// <returns>Availability status</returns>
        [HttpGet("check-mobile")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CheckMobileAvailability([FromQuery] string mobile, [FromQuery] Guid? excludeDoctorId = null)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return BadRequest(new Response(400, "Mobile number is required", null));

            var exists = await _doctorLoginService.MobileExistsAsync(mobile, excludeDoctorId);
            return Ok(new { available = !exists, mobile });
        }
    }
}

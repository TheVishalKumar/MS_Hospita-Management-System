using HospitalManagementSystem.Models.DTO.Doctors;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Doctors
{
    /// <summary>
    /// Interface for doctor authentication and profile management
    /// Supports login, registration, password change, and CRUD operations
    /// </summary>
    public interface IDoctorLoginRepository
    {
        /// <summary>
        /// Doctor login with email/mobile and password
        /// </summary>
        Task<Response> LoginAsync(DoctorLoginDto loginDto);

        /// <summary>
        /// Register a new doctor with initial password
        /// </summary>
        Task<Response> RegisterAsync(CreateDoctorLoginDto createDoctorDto);

        /// <summary>
        /// Get doctor profile by ID
        /// </summary>
        Task<DoctorProfileDto> GetDoctorProfileAsync(Guid doctorId);

        /// <summary>
        /// Update doctor profile information
        /// </summary>
        Task<Response> UpdateDoctorProfileAsync(UpdateDoctorProfileDto updateDoctorDto);

        /// <summary>
        /// Change doctor password
        /// </summary>
        Task<Response> ChangeDoctorPasswordAsync(ChangeDoctorPasswordDto changePasswordDto);

        /// <summary>
        /// Reset doctor password (Admin operation)
        /// </summary>
        Task<Response> ResetDoctorPasswordAsync(Guid doctorId, string newPassword);

        /// <summary>
        /// Update doctor password anonymously (no authorization required)
        /// </summary>
        Task<Response> UpdateDoctorPasswordAnonymousAsync(UpdateDoctorPasswordAnonymousDto dto);

        /// <summary>
        /// Get all doctors with pagination
        /// </summary>
        Task<List<DoctorProfileDto>> GetAllDoctorsAsync();

        /// <summary>
        /// Deactivate or activate a doctor account
        /// </summary>
        Task<Response> SetDoctorStatusAsync(Guid doctorId, bool isActive);

        /// <summary>
        /// Check if email already exists
        /// </summary>
        Task<bool> EmailExistsAsync(string email, Guid? excludeDoctorId = null);

        /// <summary>
        /// Check if mobile number already exists
        /// </summary>
        Task<bool> MobileExistsAsync(string mobile, Guid? excludeDoctorId = null);
    }
}

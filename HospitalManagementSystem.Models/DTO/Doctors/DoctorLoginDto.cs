using System;

namespace HospitalManagementSystem.Models.DTO.Doctors
{
    /// <summary>
    /// DTO for doctor login request
    /// </summary>
    public class DoctorLoginDto
    {
        public string? EmailId { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
    }

    /// <summary>
    /// DTO for doctor login response containing JWT token and doctor details
    /// </summary>
    public class DoctorLoginResponseDto
    {
        public Guid DoctorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? EmailId { get; set; }
        public string? MobileNo { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for creating a new doctor with login credentials
    /// </summary>
    public class CreateDoctorLoginDto
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
    }

    /// <summary>
    /// DTO for updating doctor profile
    /// </summary>
    public class UpdateDoctorProfileDto
    {
        public Guid DoctorId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for changing doctor password
    /// </summary>
    public class ChangeDoctorPasswordDto
    {
        public Guid DoctorId { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    /// <summary>
    /// DTO for updating doctor password anonymously
    /// </summary>
    public class UpdateDoctorPasswordAnonymousDto
    {
        public Guid DoctorId { get; set; }
        public string? NewPassword { get; set; }
    }

    /// <summary>
    /// DTO for doctor profile response
    /// </summary>
    public class DoctorProfileDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

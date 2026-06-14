using HospitalManagementSystem.Shared.Common;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Models.Doctors
{
    public class DoctorMaster : CommonEntity
    {
        [Key]
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
        
        /// <summary>
        /// Password hash for doctor login (MUST be hashed with BCrypt, never store plain text)
        /// </summary>
        public string? Password { get; set; }
        
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; } = 0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Users
{
    public class UpdateUserDetailsDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public DateTime DOB { get; set; }
        public string? DOJ { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public bool? IsActive { get; set; }
        public string? ProfileImage { get; set; }
        public string? Password { get; set; }
        public Guid HospitalId { get; set; }
        public Guid BranchId { get; set; }
    }
}

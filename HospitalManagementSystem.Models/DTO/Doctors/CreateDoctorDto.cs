using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Doctors
{
    public class CreateDoctorDto
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
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
    }
}

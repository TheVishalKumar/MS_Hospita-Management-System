using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Patients
{
    public class GetPatientDto : CommonEntity
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? FatherName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public DateTime DOB { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public bool? IsActive { get; set; }
        public string? ProfileImage { get; set; }
        public string? DocType { get; set; }
        public string? DocNumber { get; set; }
        public string? DocAttachment { get; set; }
        public string? Address { get; set; }
    }
}

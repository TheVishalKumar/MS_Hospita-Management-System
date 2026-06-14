using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Patients
{
    public class PatientDetails : CommonEntity
    {
        [Key]
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
        public string? Status { get; set; }
        public string? DocType { get; set; }
        public string? DocNumber { get; set; }
        public string? DocAttachment { get; set; }
        public string? Address { get; set; }
        public string? ProfileImage { get; set; }
        public Guid HospitalId { get; set; }
        public Guid BranchId { get; set; }
        public int Version { get; set; } = 0;
    }
}

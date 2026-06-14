using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Hospitals
{
    public class GetHospitalDto : CommonEntity
    {
        public Guid Id { get; set; }
        public string? HospitalName { get; set; }
        public string? Address { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public string? GSTNo { get; set; }
        public string? FaxNo { get; set; }
        public string? HospitalLogo { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}

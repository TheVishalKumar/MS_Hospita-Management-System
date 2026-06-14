using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.HospitalBranches
{
    public class UpdateHospitalBranchDto
    {
        public Guid Id { get; set; }
        public Guid HospitalId { get; set; }
        public string? BranchName { get; set; }
        public string? Address { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public string? GSTNo { get; set; }
        public string? FaxNo { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}

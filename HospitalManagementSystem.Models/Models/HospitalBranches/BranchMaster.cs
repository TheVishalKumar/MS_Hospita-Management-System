using HospitalManagementSystem.Models.Models.Hospitals;
using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.HospitalBranches
{
    public class BranchMaster : CommonEntity
    {
        [Key]
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

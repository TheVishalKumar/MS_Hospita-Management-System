using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Wards
{
    public class GetWardDto : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? WardName { get; set; }
        public string? WardDescription { get; set; }
        public bool IsActive { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Wards
{
    public class CreateWardDto
    {
        public string? WardName { get; set; }
        public string? WardDescription { get; set; }
        public bool IsActive { get; set; }
    }
}

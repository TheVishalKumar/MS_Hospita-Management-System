using HospitalManagementSystem.Shared.Common;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Models.Wards
{
    public class WardMaster : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? WardName { get; set; }
        public string? WardDescription { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; } = 0;
    }
}

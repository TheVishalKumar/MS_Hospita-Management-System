using HospitalManagementSystem.Shared.Common;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Models.Diseases
{
    public class DiseaseMaster : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? DiseaseName { get; set;}
        public string? DiseaseDescription { get; set;}
        public bool IsActive { get; set; }
        public int Version { get; set; } = 0;
    }
}

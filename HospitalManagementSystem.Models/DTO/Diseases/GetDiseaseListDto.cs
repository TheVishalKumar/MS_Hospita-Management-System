using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Diseases
{
    public class GetDiseaseListDto : CommonEntity
    {
        public Guid Id { get; set; }
        public string? DiseaseName { get; set; }
        public string? DiseaseDescription { get; set; }
        public bool IsActive { get; set; }
    }
}

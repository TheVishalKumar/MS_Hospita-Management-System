using HospitalManagementSystem.Models.DTO.Diseases;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Diseases
{
    public interface IDiseaseRepository
    {
        Task<Response> CreateAsyn(CreateDiseaseDto createDiseaseDto);
        Task<Response> UpdateAsyn(UpdateDiseaseDto updateDiseaseDto);
        Task<List<GetDiseaseListDto>> GetListAsync();
        Task<GetDiseaseListDto> GetById(Guid id);
        Task<bool> UpdateStatus(Guid id, bool status);
        Task<GetDiseaseListDto> GetByName(string name);
        Task<object> CheckDuplicateForEdit(Guid id, string name);

    }
}

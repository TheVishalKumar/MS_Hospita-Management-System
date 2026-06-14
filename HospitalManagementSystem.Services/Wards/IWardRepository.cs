using HospitalManagementSystem.Models.DTO.Wards;
using HospitalManagementSystem.Shared.Common.CommonResponse;

namespace HospitalManagementSystem.Services
{
    public interface IWardRepository
    {
        Task<Response> CreateAsyn(CreateWardDto createWardDto);
        Task<Response> UpdateAsyn(Guid id, UpdateWardDto updateWardDto);
        Task<List<GetWardDto>> GetListAsync();
        Task<List<GetWardDto>> GetActiveListAsync();
        Task<GetWardDto> GetById(Guid id);
        Task<bool> UpdateStatus(Guid id, bool status,Guid updateBy);
        Task<GetWardDto> GetByName(string name);
        Task<object> CheckDuplicateForEdit(Guid id, string name);
    }
}

using HospitalManagementSystem.Models.DTO.Hospitals;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Hospitals
{
    public interface IHospitalRepository
    {
        Task<Response> CreateAsync(CreateHospitalDto createHospitalDto);
        Task<Response> UpdateAsync(Guid id, UpdateHospitalDto updateHospitalDto);
        Task<GetHospitalDto> Get(Guid id);
        Task<List<GetHospitalDto>> GetList();
        Task<GetHospitalDto> GetByName(string name);
        Task<GetHospitalDto> GetByMobile(string mobile);
        Task<GetHospitalDto> GetByEmail(string emailId);
        Task<object> CheckDuplicateForEdit(Guid id, string name);
    }
}

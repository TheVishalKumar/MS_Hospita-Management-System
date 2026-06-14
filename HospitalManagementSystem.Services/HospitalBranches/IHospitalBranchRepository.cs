using HospitalManagementSystem.Models.DTO.HospitalBranches;
using HospitalManagementSystem.Models.DTO.Hospitals;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.HospitalBranches
{
    public interface IHospitalBranchRepository
    {
        Task<Response> CreateAsync(CreateHospitalBranchDto createHospitalBranchDto);
        Task<Response> UpdateAsync(Guid id, UpdateHospitalBranchDto updateHospitalBranchDto);
        Task<object> Get(Guid id);
        Task<object> GetList();
        Task<object> GetByName(string name);
        Task<object> GetByMobile(string mobile);
        Task<object> GetByEmail(string emailId);
        Task<object> CheckDuplicateForEdit(Guid id, string name);
    }
}

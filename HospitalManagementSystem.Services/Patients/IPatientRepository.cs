using HospitalManagementSystem.Models.DTO.Patients;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Patients
{
    public interface IPatientRepository
    {
        Task<Response> CreateAsync(CreatePatientDto createPatientDto);
        Task<Response> UpdatePatient(Guid id, UpdatePatientDto updatePatientDto);
        Task<GetPatientDto> Get(Guid id);
        Task<List<GetPatientDto>> GetList();
        Task<List<GetPatientDto>> GetpatientByHospitalBranchList(Guid hospitalId, Guid branchId);
        Task<GetPatientDto> GetByName(string name);
        Task<GetPatientDto> GetByMobile(string mobile);
        Task<GetPatientDto> GetByEmail(string emailId);
    }
}

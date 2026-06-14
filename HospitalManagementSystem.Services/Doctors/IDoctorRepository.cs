using HospitalManagementSystem.Models.DTO.Doctors;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Doctors
{
    public interface IDoctorRepository
    {
        Task<Response> CreateAsync(CreateDoctorDto createDoctorDto);
        Task<Response> UpdateAsync(Guid id, UpdateDoctorDto updateDoctorDto);
        Task<GetDoctorDto> Get(Guid id);
        Task<List<GetDoctorDto>> GetList();
        Task<List<GetDoctorDto>> GetActiveDoctorList();
        Task<GetDoctorDto> GetByName(string name);
        Task<GetDoctorDto> GetByMobile(string mobile);
        Task<GetDoctorDto> GetByEmail(string emailId);
        Task<bool> UpdateStatus(Guid id, bool verified, Guid updateBy);
        Task<object> CheckDuplicateForEdit(Guid id, string name);
    }
}

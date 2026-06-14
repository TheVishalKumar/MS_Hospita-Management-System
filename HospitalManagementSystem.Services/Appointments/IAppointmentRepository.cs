using HospitalManagementSystem.Models.DTO.Appointments;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Appointments
{
    public interface IAppointmentRepository
    {
        Task<Response> CreateAsync(CreateAppointmentDto createAppointmentDto);
        //Task<Response> UpdateUser(Guid id, UpdateAppointmentDto updateAppointmentDto);
        Task<GetAppointmentDto> Get(Guid id);
        Task<List<GetAppointmentDto>> GetList();
        Task<List<GetAppointmentDto>> GetAppointmentListByHospitalIdBranchId(Guid hospitalId, Guid branchId);
        Task<List<GetAppointmentDto>> GetTodayAppointmentListByPatient(Guid patientId, Guid HospitalId, Guid BranchId);

        Task<object> GetAppointmentListByPatientAndDate(Guid patientId, DateTime fromDate, DateTime toDate, Guid HospitalId, Guid BranchId);
        Task<object> GetAppointmentListByDates(DateTime fromDate, DateTime toDate, Guid HospitalId, Guid BranchId);
        Task<object> GetTodayAppointmentList(Guid HospitalId, Guid BranchId);
        Task<bool> UpdateStatus(Guid id, string status, Guid updateBy);
    }
}

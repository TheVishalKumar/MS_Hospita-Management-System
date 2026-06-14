using HospitalManagementSystem.Models.DTO.Appointments;
using HospitalManagementSystem.Models.DTO.Doctors;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Appointments;
using HospitalManagementSystem.Services.Doctors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystems.API.Helpers;

namespace HospitalManagementSystems.API.Controllers.Appointments
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpPost]
        [Route("CreateAppointment")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor)]
        public async Task<IActionResult> CreateUser(CreateAppointmentDto createDoctorDto)
        {
            try
            {
                var result = await _appointmentRepository.CreateAsync(createDoctorDto);
                return this.CreatedResponse(result, "Appointment created successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error creating appointment: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAppointmentList")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor + "," + UserRoles.Employee)]
        public async Task<IActionResult> GetAppointmentListAsync()
        {
            try
            {
                var data = await _appointmentRepository.GetList();
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Appointment list retrieved successfully");
                }
                return this.NotFoundResponse<List<GetAppointmentDto>>(null, "No appointments found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetAppointmentDto>>(null, $"Error retrieving appointment list: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAppointmentListByPatient/{patientId}/{HospitalId}/{BranchId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor + "," + UserRoles.Employee)]
        public async Task<IActionResult> GetAppointmentListByPatient(Guid patientId, Guid HospitalId, Guid BranchId)
        {
            try
            {
                var data = await _appointmentRepository.GetTodayAppointmentListByPatient(patientId, HospitalId, BranchId);
                if (data?.Count > 0)
                {
                    return this.OkResponse(data, "Patient appointments retrieved successfully");
                }
                return this.NotFoundResponse<List<GetAppointmentDto>>(null, "No appointments found for this patient");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetAppointmentDto>>(null, $"Error retrieving appointments: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAppointmentListByPatientAndDate/{patientId}/{fromDate}/{toDate}/{HospitalId}/{BranchId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetAppointmentListByPatientAndDate(Guid patientId, DateTime fromDate, DateTime toDate, Guid HospitalId, Guid BranchId)
        {
            try
            {
                var data = await _appointmentRepository.GetAppointmentListByPatientAndDate(patientId, fromDate, toDate, HospitalId, BranchId);
                if (data != null)
                {
                    return this.OkResponse(data, "Appointments for date range retrieved successfully");
                }
                return this.NotFoundResponse<object>(null, "No appointments found for this date range");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error retrieving appointments: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetTodayAppointmentList/{HospitalId}/{BranchId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetTodayAppointmentList(Guid HospitalId, Guid BranchId)
        {
            try
            {
                var data = await _appointmentRepository.GetTodayAppointmentList(HospitalId, BranchId);
                if (data != null)
                {
                    return this.OkResponse(data, "Today's appointments retrieved successfully");
                }
                return this.NotFoundResponse<object>(null, "No appointments for today");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error retrieving appointments: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAppointmentListByDates/{fromDate}/{toDate}/{HospitalId}/{BranchId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetPatientListDate(DateTime fromDate, DateTime toDate, Guid HospitalId, Guid BranchId)
        {
            try
            {
                var data = await _appointmentRepository.GetAppointmentListByDates(fromDate, toDate, HospitalId, BranchId);
                if (data != null)
                {
                    return this.OkResponse(data, "Appointments for date range retrieved successfully");
                }
                return this.NotFoundResponse<object>(null, "No appointments found for this date range");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error retrieving appointments: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAppointmentById/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            try
            {
                var data = await _appointmentRepository.Get(id);
                if (data != null)
                {
                    return this.OkResponse(data, "Appointment retrieved successfully");
                }
                return this.NotFoundResponse<GetAppointmentDto>(null, "Appointment not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetAppointmentDto>(null, $"Error retrieving appointment: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("UpdateStatus/{id}/{verified}/{updateBy}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor)]
        public async Task<IActionResult> UpdateStatus(Guid id, string status, Guid updateBy)
        {
            try
            {
                var result = await _appointmentRepository.UpdateStatus(id, status, updateBy);
                if (result)
                {
                    return this.OkResponse(result, "Appointment status updated successfully");
                }
                return this.BadRequestResponse<bool>(false, "Failed to update appointment status");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<bool>(false, $"Error updating appointment status: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAppointmentListByHospitalIdBranchId/{hospitalId}/{branchId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor + "," + UserRoles.Employee)]
        public async Task<IActionResult> GetAppointmentListByHospitalIdBranchId(Guid hospitalId, Guid branchId)
        {
            try
            {
                var data = await _appointmentRepository.GetAppointmentListByHospitalIdBranchId(hospitalId, branchId);
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Appointments for hospital and branch retrieved successfully");
                }
                return this.NotFoundResponse<List<GetAppointmentDto>>(null, "No appointments found for this hospital and branch");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetAppointmentDto>>(null, $"Error retrieving appointments: {ex.Message}");
            }
        }
    }
}

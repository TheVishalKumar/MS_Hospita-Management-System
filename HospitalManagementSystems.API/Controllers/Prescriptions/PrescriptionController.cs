using FluentValidation;
using HospitalManagementSystem.Models.DTO.Patients;
using HospitalManagementSystem.Models.DTO.Prescriptions;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Patients;
using HospitalManagementSystem.Services.Prescriptions;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystems.API.Helpers;

namespace HospitalManagementSystems.API.Controllers.Prescriptions
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPatientPrescription _patientPrescriptionRepository;
        public PrescriptionController(IPatientPrescription patientPrescriptionRepository)
        {
            _patientPrescriptionRepository = patientPrescriptionRepository;
        }

        [HttpPost]
        [Route("CreatePatientPrescription")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Doctor)]
        public async Task<IActionResult> CreatePatientPrescription(CreatePatientPrescriptionDto createPatientPrescriptionDto)
        {
            try
            {
                var result = await _patientPrescriptionRepository.CreateAsync(createPatientPrescriptionDto);
                return this.CreatedResponse(result, "Prescription created successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<Response>(null, $"Error creating prescription: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetPatientPrescription")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Doctor + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetPatientPrescriptionAsync()
        {
            try
            {
                var data = await _patientPrescriptionRepository.GetList();
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Prescriptions retrieved successfully");
                }
                return this.NotFoundResponse<List<GetPatientPrescriptionDto>>(null, "No prescriptions found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetPatientPrescriptionDto>>(null, $"Error retrieving prescriptions: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Doctor + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var data = await _patientPrescriptionRepository.Get(id);
                if (data != null)
                {
                    return this.OkResponse(data, "Prescription retrieved successfully");
                }
                return this.NotFoundResponse<GetPatientPrescriptionDto>(null, "Prescription not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetPatientPrescriptionDto>(null, $"Error retrieving prescription: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetByAppointmentId/{appointmentId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Doctor + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetByAppointmentId(Guid appointmentId)
        {
            try
            {
                var data = await _patientPrescriptionRepository.GetByAppointmentId(appointmentId);
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Prescriptions for appointment retrieved successfully");
                }
                return this.NotFoundResponse<List<GetPatientPrescriptionDto>>(null, "No prescriptions found for this appointment");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetPatientPrescriptionDto>>(null, $"Error retrieving prescriptions: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetByPatientId/{patientId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Doctor + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetByPatientId(Guid patientId)
        {
            try
            {
                var data = await _patientPrescriptionRepository.GetByPatientId(patientId);
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Prescriptions for patient retrieved successfully");
                }
                return this.NotFoundResponse<List<GetPatientPrescriptionDto>>(null, "No prescriptions found for this patient");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetPatientPrescriptionDto>>(null, $"Error retrieving prescriptions: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetByHospitalAndBranch/{hospitalId}/{branchId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Doctor + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetByHospitalAndBranch(Guid hospitalId, Guid branchId)
        {
            try
            {
                var data = await _patientPrescriptionRepository.GetPrescriptionsByHospitalAndBranch(hospitalId, branchId);
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Prescriptions for hospital and branch retrieved successfully");
                }
                return this.NotFoundResponse<List<GetPatientPrescriptionDto>>(null, "No prescriptions found for this hospital and branch");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetPatientPrescriptionDto>>(null, $"Error retrieving prescriptions: {ex.Message}");
            }
        }

    }
}

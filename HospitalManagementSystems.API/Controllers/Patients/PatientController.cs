using FluentValidation;
using HospitalManagementSystem.Models.DTO.Patients;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Patients;
using HospitalManagementSystem.Services.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystems.API.Helpers;

namespace HospitalManagementSystems.API.Controllers.Patients
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpPost]
        [Route("CreatePatient")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist)]
        public async Task<IActionResult> CreatePatient(CreatePatientDto createPatientDto)
        {
            try
            {
                string error = "";
                PatientDetailsValidation validations = new PatientDetailsValidation();
                var result = validations.Validate(createPatientDto);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }

                var data = await _patientRepository.GetByMobile(createPatientDto.MobileNo ?? "");

                if (data != null)
                {
                    return this.BadRequestResponse<object>(null, $"Patient already exist with same mobile no : '{createPatientDto.MobileNo}'");
                }

                var createResult = await _patientRepository.CreateAsync(createPatientDto);
                return this.CreatedResponse(createResult, "Patient created successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error creating patient: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdatePatient/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist)]
        public async Task<IActionResult> UpdatePatient(Guid id, UpdatePatientDto updatePatientDto)
        {
            try
            {
                string error = "";
                UpdatePatientDetailsValidation validations = new UpdatePatientDetailsValidation();
                var result = validations.Validate(updatePatientDto);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }

                var data = await _patientRepository.GetByMobile(updatePatientDto.MobileNo ?? "");

                if (data != null && data.Id != id)
                {
                    return this.BadRequestResponse<object>(null, $"Patient already exist with same mobile no : '{updatePatientDto.MobileNo}'");
                }

                var updateResult = await _patientRepository.UpdatePatient(id, updatePatientDto);
                return this.OkResponse(updateResult, "Patient updated successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error updating patient: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetPatientList")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor + "," + UserRoles.Employee + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetPatientListAsync()
        {
            try
            {
                var data = await _patientRepository.GetList();
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Patient list retrieved successfully");
                }
                return this.NotFoundResponse<List<GetPatientDto>>(null, "No patients found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetPatientDto>>(null, $"Error retrieving patient list: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetpatientByHospitalBranchList/{hospitalId}/{branchId}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetpatientByHospitalBranchList(Guid hospitalId, Guid branchId)
        {
            try
            {
                var data = await _patientRepository.GetpatientByHospitalBranchList(hospitalId, branchId);
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Patients retrieved successfully");
                }
                return this.NotFoundResponse<List<GetPatientDto>>(null, "No patients found for this hospital branch");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetPatientDto>>(null, $"Error retrieving patients: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetByMobile/{mobile}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor + "," + UserRoles.Employee + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetByMobile(string mobile)
        {
            try
            {
                var data = await _patientRepository.GetByMobile(mobile);
                if (data != null)
                {
                    return this.OkResponse(data, "Patient retrieved successfully");
                }
                return this.NotFoundResponse<GetPatientDto>(null, "Patient not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetPatientDto>(null, $"Error retrieving patient: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor + "," + UserRoles.Employee + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var data = await _patientRepository.Get(id);
                if (data != null)
                {
                    return this.OkResponse(data, "Patient retrieved successfully");
                }
                return this.NotFoundResponse<GetPatientDto>(null, "Patient not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetPatientDto>(null, $"Error retrieving patient: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor + "," + UserRoles.Employee + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var data = await _patientRepository.GetByName(name);
                if (data != null)
                {
                    return this.OkResponse(data, "Patient retrieved successfully");
                }
                return this.NotFoundResponse<GetPatientDto>(null, "Patient not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetPatientDto>(null, $"Error retrieving patient: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetByEmail/{email}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor + "," + UserRoles.Employee + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var data = await _patientRepository.GetByEmail(email);
                if (data != null)
                {
                    return this.OkResponse(data, "Patient retrieved successfully");
                }
                return this.NotFoundResponse<GetPatientDto>(null, "Patient not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetPatientDto>(null, $"Error retrieving patient: {ex.Message}");
            }
        }
    }
}

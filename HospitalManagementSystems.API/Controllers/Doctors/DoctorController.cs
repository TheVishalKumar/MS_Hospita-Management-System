using HospitalManagementSystem.Models.DTO.Doctors;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Doctors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystems.API.Helpers;

namespace HospitalManagementSystems.API.Controllers.Doctors
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;
        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpPost]
        [Route("CreateDoctor")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateUser(CreateDoctorDto createDoctorDto)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(createDoctorDto.MobileNo))
                {
                    var data = await _doctorRepository.GetByMobile(createDoctorDto.MobileNo);
                    if (data != null)
                    {
                        return this.BadRequestResponse<object>(null, $"Doctor already exist with same mobile no : '{createDoctorDto.MobileNo}'");
                    }
                }

                DoctorValidation validations = new DoctorValidation();
                string error = "";
                var result = validations.Validate(createDoctorDto);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }

                var createResult = await _doctorRepository.CreateAsync(createDoctorDto);
                return this.CreatedResponse(createResult, "Doctor created successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error creating doctor: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateDoctor/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateDoctorDto updateDoctorDto)
        {
            try
            {
                UpdateDoctorValidation validations = new UpdateDoctorValidation();
                string error = "";
                var result = validations.Validate(updateDoctorDto);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }

                if (!string.IsNullOrWhiteSpace(updateDoctorDto.MobileNo))
                {
                    var data = await _doctorRepository.CheckDuplicateForEdit(id, updateDoctorDto.MobileNo);
                    if (data != null)
                    {
                        return this.BadRequestResponse<object>(null, $"Doctor already exist with same mobile no : '{updateDoctorDto.MobileNo}'");
                    }
                }

                var updateResult = await _doctorRepository.UpdateAsync(id, updateDoctorDto);
                return this.OkResponse(updateResult, "Doctor updated successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error updating doctor: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetDoctorList")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist)]
        public async Task<IActionResult> GetDoctorListAsync()
        {
            try
            {
                var data = await _doctorRepository.GetList();
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Doctor list retrieved successfully");
                }
                return this.NotFoundResponse<List<GetDoctorDto>>(null, "No doctors found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetDoctorDto>>(null, $"Error retrieving doctor list: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetActiveDoctorList")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist)]
        public async Task<IActionResult> GetActiveDoctorList()
        {
            try
            {
                var data = await _doctorRepository.GetActiveDoctorList();
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Active doctor list retrieved successfully");
                }
                return this.NotFoundResponse<List<GetDoctorDto>>(null, "No active doctors found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetDoctorDto>>(null, $"Error retrieving active doctor list: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetDoctorById/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            try
            {
                var data = await _doctorRepository.Get(id);
                if (data != null)
                {
                    return this.OkResponse(data, "Doctor retrieved successfully");
                }
                return this.NotFoundResponse<GetDoctorDto>(null, "Doctor not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetDoctorDto>(null, $"Error retrieving doctor: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("UpdateStatus/{id}/{verified}/{updateBy}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateStatus(Guid id, bool verified, Guid updateBy)
        {
            try
            {
                var result = await _doctorRepository.UpdateStatus(id, verified, updateBy);
                if (result)
                {
                    return this.OkResponse(result, "Doctor status updated successfully");
                }
                return this.BadRequestResponse<bool>(false, "Failed to update doctor status");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<bool>(false, $"Error updating doctor status: {ex.Message}");
            }
        }
    }
}

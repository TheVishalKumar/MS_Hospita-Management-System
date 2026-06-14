using FluentValidation;
using HospitalManagementSystem.Models.DTO.HospitalBranches;
using HospitalManagementSystem.Models.DTO.Hospitals;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Hospitals;
using HospitalManagementSystem.Services.Medicines;
using HospitalManagementSystem.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystems.API.Helpers;

namespace HospitalManagementSystems.API.Controllers.Hospitals
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalRepository _hospitalRepository;
        public HospitalController(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        [HttpPost]
        [Route("CreateHospital")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateAsync(CreateHospitalDto createHospitalDto)
        {
            try
            {
                var data = await _hospitalRepository.GetByName(createHospitalDto.HospitalName ?? "");
                if (data != null)
                {
                    return this.BadRequestResponse<object>(null, $"Hospital already exist with same name : '{createHospitalDto.HospitalName}'.");
                }

                HospitalValidation validations = new HospitalValidation();
                string error = "";
                var result = validations.Validate(createHospitalDto);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }

                var createResult = await _hospitalRepository.CreateAsync(createHospitalDto);
                return this.CreatedResponse(createResult, "Hospital created successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error creating hospital: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateHospital/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateHospitalDto updateHospitalDto)
        {
            try
            {
                UpdateHospitalValidation validations = new UpdateHospitalValidation();
                string error = "";
                var result = validations.Validate(updateHospitalDto);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }

                var checkDuplication = await _hospitalRepository.CheckDuplicateForEdit(id, updateHospitalDto.HospitalName ?? "");

                if (checkDuplication != null)
                {
                    return this.BadRequestResponse<object>(null, $"Hospital already exist with same name : {updateHospitalDto.HospitalName}");
                }

                var updateResult = await _hospitalRepository.UpdateAsync(id, updateHospitalDto);
                return this.OkResponse(updateResult, "Hospital updated successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error updating hospital: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetHospital")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist)]
        public async Task<IActionResult> GetUserListAsync()
        {
            try
            {
                var data = await _hospitalRepository.GetList();
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Hospital list retrieved successfully");
                }
                return this.NotFoundResponse<List<GetHospitalDto>>(null, "No hospitals found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetHospitalDto>>(null, $"Error retrieving hospital list: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetHospital/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Receptionist)]
        public async Task<IActionResult> GetHospitalAsync(Guid id)
        {
            try
            {
                var data = await _hospitalRepository.Get(id);
                if (data != null)
                {
                    return this.OkResponse(data, "Hospital retrieved successfully");
                }
                return this.NotFoundResponse<GetHospitalDto>(null, "Hospital not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetHospitalDto>(null, $"Error retrieving hospital: {ex.Message}");
            }
        }
    }
}

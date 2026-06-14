using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.DTO.Medicines;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Categories;
using HospitalManagementSystem.Services.Medicines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystems.API.Helpers;

namespace HospitalManagementSystems.API.Controllers.Medicines
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineRepository _medicineRepository;
        public MedicineController(IMedicineRepository medicineRepository)
        {
            _medicineRepository= medicineRepository;
        }

        [HttpPost]
        [Route("CreateMedicine")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> CreateAsync(CreateMedicineDto createMedicineDto)
        {
            try
            {
                string? error = "";
                if (!string.IsNullOrWhiteSpace(createMedicineDto.MedicineName))
                {
                    var data = await _medicineRepository.GetByName(createMedicineDto.MedicineName);
                    if (data != null)
                    {
                        return this.BadRequestResponse<object>(null, $"Medicine already existed with same name : {createMedicineDto.MedicineName}");
                    }
                }
                CreateMedicineValidations validations = new CreateMedicineValidations();
                var result = validations.Validate(createMedicineDto);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }

                var createResult = await _medicineRepository.CreateAsync(createMedicineDto);
                return this.CreatedResponse(createResult, "Medicine created successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error creating medicine: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetList")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.PharmacyStaff + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var data = await _medicineRepository.GetAsync();
                if (data != null)
                {
                    return this.OkResponse(data, "Medicine list retrieved successfully");
                }
                return this.NotFoundResponse<object>(null, "No medicines found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error retrieving medicines: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetExpiryMedicines")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetExpiryMedicines([FromQuery] int days = 30)
        {
            try
            {
                // Returns medicines with ExpiryDate within the next `days` days (inclusive)
                var data = await _medicineRepository.GetExpiryMedicines(days);
                if (data != null)
                {
                    return this.OkResponse(data, $"Medicines expiring within {days} days retrieved successfully");
                }
                return this.NotFoundResponse<object>(null, "No medicines expiring soon");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error retrieving expiring medicines: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.PharmacyStaff + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _medicineRepository.GetById(id);
                if (data != null)
                {
                    return this.OkResponse(data, "Medicine retrieved successfully");
                }
                return this.NotFoundResponse<object>(null, "Medicine not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error retrieving medicine: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateMedicine/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateMedicineDto createUpdateCategory)
        {
            try
            {
                string? error = "";
                UpdateMedicineValidations validations = new UpdateMedicineValidations();
                var result = validations.Validate(createUpdateCategory);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }
                if (!string.IsNullOrWhiteSpace(createUpdateCategory.MedicineName))
                {
                    var checkDuplication = await _medicineRepository.CheckDuplicateForEdit(id, createUpdateCategory.MedicineName);

                    if (checkDuplication != null)
                    {
                        return this.BadRequestResponse<object>(null, $"Medicine already exist with same name : {createUpdateCategory.MedicineName}");
                    }
                }
                var updateResult = await _medicineRepository.UpdateAsync(id, createUpdateCategory);
                return this.OkResponse(updateResult, "Medicine updated successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error updating medicine: {ex.Message}");
            }
        }
    }
}

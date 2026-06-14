using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.DTO.Medicines;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Categories;
using HospitalManagementSystem.Services.Medicines;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HospitalManagementSystems.API.Controllers.Medicines
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineRepository _medicineRepository;
        public MedicineController(IMedicineRepository medicineRepository)
        {
            _medicineRepository= medicineRepository;
        }

        [HttpPost]
        [Route("CreateMedicine")]
        public async Task<object> CreateAsync(CreateMedicineDto createMedicineDto)
        {
            string? error = "";
            var data =  _medicineRepository.GetByName(createMedicineDto.MedicineName);
            if (data.Result != null)
            {
                throw new DuplicateNameException($"Medicine already existed with same name : {createMedicineDto.MedicineName}");
            }
            CreateMedicineValidations validations = new CreateMedicineValidations();
            var result = validations.Validate(createMedicineDto);
            if (!result.IsValid)
            {
                foreach(var item in result.Errors) 
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }

            return await _medicineRepository.CreateAsync(createMedicineDto);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<object> GetAsync()
        { 
            return await _medicineRepository.GetAsync();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<object> GetByIdAsync(Guid id)
        {
            return await _medicineRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateMedicine/{id}")]
        public async Task<object> UpdateCategory(Guid id, UpdateMedicineDto createUpdateCategory)
        {
            string? error = "";
            UpdateMedicineValidations validations = new UpdateMedicineValidations();
            var result = validations.Validate(createUpdateCategory);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }
            var checkDuplication = _medicineRepository.CheckDuplicateForEdit(id, createUpdateCategory.MedicineName);

            if (checkDuplication.Result != null)
            {
                throw new DuplicationRecordException($"Medicine already exist with same name : {createUpdateCategory.MedicineName}");
            }
            return await _medicineRepository.UpdateAsync(id, createUpdateCategory);
        }

        // New endpoint: Get medicines that will expire within the next 'days' days.
        // Example: GET api/Medicine/GetExpiring?days=30
        [HttpGet]
        [Route("GetExpiring")]
        public async Task<object> GetExpiringAsync([FromQuery] int days = 30)
        {
            if (days <= 0) days = 30;
            return await _medicineRepository.GetExpiringMedicinesAsync(days);
        }
    }
}

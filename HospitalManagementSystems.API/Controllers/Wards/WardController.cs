using HospitalManagementSystem.Models.DTO.Wards;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Services.Doctors;
using HospitalManagementSystem.Services.Wards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HospitalManagementSystems.API.Controllers.Wards
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    
    public class WardController : ControllerBase
    {
        private readonly IWardRepository _WardRepository;
        public WardController(IWardRepository WardRepository)
        {
            _WardRepository = WardRepository;
        }

        [HttpPost]
        [Route("CreateWard")]
        public async Task<object> CreateAsync(CreateWardDto createWardDto)
        {
            string? error = "";
            var data = _WardRepository.GetByName(createWardDto.WardName);
            if (data != null)
            {
                throw new DuplicateNameException($"Ward already existed with same name : {createWardDto.WardName}");
            }
            WardValidation validations = new WardValidation();
            var result = validations.Validate(createWardDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }

            return await _WardRepository.CreateAsyn(createWardDto);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<object> GetAsync()
        {
            return await _WardRepository.GetListAsync();
        }

        [HttpGet]
        [Route("GetActiveWardList")]
        public async Task<object> GetActiveListAsync()
        {
            return await _WardRepository.GetActiveListAsync();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<object> GetByIdAsync(Guid id)
        {
            return await _WardRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateWard/{id}")]
        public async Task<object> UpdateCategory(Guid id, UpdateWardDto updateWardDto)
        {
            string? error = "";
            UpdateWardValidation validations = new UpdateWardValidation();
            var result = validations.Validate(updateWardDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }
            var checkDuplication = _WardRepository.CheckDuplicateForEdit(id, updateWardDto.WardName);

            if (checkDuplication != null)
            {
                throw new DuplicationRecordException($"Ward already exist with same name : {updateWardDto.WardName}");
            }
            return await _WardRepository.UpdateAsyn(id,updateWardDto);
        }

        [HttpGet]
        [Route("UpdateStatus/{id}/{verified}/{updateBy}")]
        public async Task<bool> UpdateStatus(Guid id, bool verified, Guid updateBy)
        {
            return await _WardRepository.UpdateStatus(id, verified, updateBy);
        }
    }
}

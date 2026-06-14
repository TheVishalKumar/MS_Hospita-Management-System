using HospitalManagementSystem.Models.DTO.Diseases;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Diseases;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HospitalManagementSystems.API.Controllers.Diseases
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseRepository _diseaseRepository;
        public DiseaseController(IDiseaseRepository diseaseRepository)
        {
            _diseaseRepository= diseaseRepository;
        }

        [HttpPost]
        [Route("CreateDisease")]
        public async Task<object> CreateAsync(CreateDiseaseDto createDiseaseDto)
        {
            string? error = "";
            var data = await _diseaseRepository.GetByName(createDiseaseDto.DiseaseName ?? "");
            if (data != null)
            {
                throw new DuplicateNameException($"Disease already existed with same name : {createDiseaseDto.DiseaseName}");
            }
            DiseaseValidation validations = new DiseaseValidation();
            var result = validations.Validate(createDiseaseDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }

            return await _diseaseRepository.CreateAsyn(createDiseaseDto);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<object> GetAsync()
        {
            return await _diseaseRepository.GetListAsync();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<object> GetByIdAsync(Guid id)
        {
            return await _diseaseRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateDisease/{id}")]
        public async Task<object> UpdateCategory(Guid id, UpdateDiseaseDto updateDiseaseDto)
        {
            string? error = "";
            UpdateDiseaseValidation validations = new UpdateDiseaseValidation();
            var result = validations.Validate(updateDiseaseDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }
            var checkDuplication = await _diseaseRepository.CheckDuplicateForEdit(id, updateDiseaseDto.DiseaseName ?? "");

            if (checkDuplication != null)
            {
                throw new DuplicationRecordException($"Disease already exist with same name : {updateDiseaseDto.DiseaseName}");
            }
            return await _diseaseRepository.UpdateAsyn(updateDiseaseDto);
        }

    }
}

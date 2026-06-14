using FluentValidation;
using HospitalManagementSystem.Models.DTO.HospitalBranches;
using HospitalManagementSystem.Models.DTO.Hospitals;
using HospitalManagementSystem.Models.DTO.Wards;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.HospitalBranches;
using HospitalManagementSystem.Services.Hospitals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystems.API.Controllers.HospitalBranches
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    
    public class HospitalBranchController : ControllerBase
    {
        private readonly IHospitalBranchRepository _hospitalBranchRepository;
        private readonly IHospitalRepository _hospitalRepository;
        public HospitalBranchController(IHospitalRepository hospitalRepository ,IHospitalBranchRepository hospitalBranchRepository)
        {
            _hospitalBranchRepository = hospitalBranchRepository;
            _hospitalRepository = hospitalRepository;
        }

        [HttpPost]
        [Route("CreateBranch")]
        public async Task<object> CreateAsync(CreateHospitalBranchDto createHospitalBranchDto)
        {
            var data = await _hospitalBranchRepository.GetByName(createHospitalBranchDto.BranchName ?? "");
            if (data != null)
            {
                throw new DuplicationRecordException($"Branch already exist with same name : '{createHospitalBranchDto.BranchName}'.");
            }

            var isExist = _hospitalRepository.Get(createHospitalBranchDto.HospitalId);
            if (isExist.Result == null) 
            {
                throw new NotFoundException("Selected hospital Not Exist in our database.");
            }

            HospitalBranchValidation validations = new HospitalBranchValidation();
            string error = "";
            var result = validations.Validate(createHospitalBranchDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }

            return await _hospitalBranchRepository.CreateAsync(createHospitalBranchDto);
        }

        [HttpPut]
        [Route("UpdateBranch/{id}")]
        public async Task<object> UpdateAsync(Guid id, UpdateHospitalBranchDto updateHospitalBranchDto)
        {
            UpdateHospitalBranchValidation validations = new UpdateHospitalBranchValidation();
            string error = "";
            var result = validations.Validate(updateHospitalBranchDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }

            var checkDuplication = await _hospitalBranchRepository.CheckDuplicateForEdit(id, updateHospitalBranchDto.BranchName ?? "");

            if (checkDuplication != null)
            {
                throw new DuplicationRecordException($"Branch already exist with same name : {updateHospitalBranchDto.BranchName}");
            }

            var isExist = _hospitalBranchRepository.Get(updateHospitalBranchDto.HospitalId);
            if (isExist.Result == null)
            {
                throw new NotFoundException("Selected hospital Not Exist in our database.");
            }

            return await _hospitalBranchRepository.UpdateAsync(id, updateHospitalBranchDto);
        }

        [HttpGet]
        [Route("GetBranch")]
        public async Task<object> GetUserListAsync()
        {
            var data = await _hospitalBranchRepository.GetList();
            if (data != null)
            {
                return await _hospitalBranchRepository.GetList();
            }
            throw new NotFoundException("Data Not found!");

        }

        [HttpGet]
        [Route("GetBranch/{id}")]
        public async Task<object> GetHospitalAsync(Guid id)
        {
            var data = await _hospitalBranchRepository.GetList();
            if (data != null)
            {
                return await _hospitalBranchRepository.Get(id);
            }
            throw new NotFoundException("Data Not found!");

        }
    }
}

using HospitalManagementSystem.Models.DTO.Rooms;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Rooms;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HospitalManagementSystems.API.Controllers.Rooms
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _RoomRepository;
        public RoomController(IRoomRepository RoomRepository)
        {
            _RoomRepository = RoomRepository;
        }

        [HttpPost]
        [Route("CreateRoom")]
        public async Task<object> CreateAsync(CreateRoomDto createRoomDto)
        {
            string? error = "";
            var data = _RoomRepository.GetByName(createRoomDto.RoomName);
            if (data != null)
            {
                throw new DuplicateNameException($"Room already existed with same name : {createRoomDto.RoomName}");
            }
            RoomValidation validations = new RoomValidation();
            var result = validations.Validate(createRoomDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }

            return  await _RoomRepository.CreateAsyn(createRoomDto);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<object> GetAsync()
        {
            return await _RoomRepository.GetListAsync();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<object> GetByIdAsync(Guid id)
        {
            return await _RoomRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateRoom/{id}")]
        public async Task<object> UpdateCategory(Guid id, UpdateRoomDto updateRoomDto)
        {
            string? error = "";
            UpdateRoomValidation validations = new UpdateRoomValidation();
            var result = validations.Validate(updateRoomDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    error += item.ErrorMessage + "\n\n";
                }

                return BadRequest(error);
            }
            var checkDuplication = _RoomRepository.CheckDuplicateForEdit(id, updateRoomDto.RoomName);

            if (checkDuplication != null)
            {
                throw new DuplicationRecordException($"Room already exist with same name : {updateRoomDto.RoomName}");
            }
            return await _RoomRepository.UpdateAsyn(updateRoomDto);
        }
    }
}

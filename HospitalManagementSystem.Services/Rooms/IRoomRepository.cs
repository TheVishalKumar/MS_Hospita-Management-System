using HospitalManagementSystem.Models.DTO.Rooms;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Rooms
{
    public interface IRoomRepository
    {
        Task<Response> CreateAsyn(CreateRoomDto createRoomDto);
        Task<Response> UpdateAsyn(UpdateRoomDto updateRoomDto);
        Task<List<GetRoomDto>> GetListAsync();
        Task<GetRoomDto> GetById(Guid id);
        Task<bool> UpdateStatus(Guid id, bool status);
        Task<GetRoomDto> GetByName(string name);
        Task<object> CheckDuplicateForEdit(Guid id, string name);
    }
}

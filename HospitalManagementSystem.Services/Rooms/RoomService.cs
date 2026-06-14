using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Rooms;
using HospitalManagementSystem.Models.Models.Rooms;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services.Rooms
{
    public class RoomService : IRoomRepository
    {
        private readonly AppDbContext _appDbContext;
        private IMapper _mapper;
        public RoomService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;

        }
        public async Task<Response> CreateAsyn(CreateRoomDto createRoomDto)
        {
            try
            {
                var Room = _mapper.Map<RoomMaster>(createRoomDto);
                Room.Id = Guid.NewGuid();
                Room.CreatedDate = DateTime.Now;
                await _appDbContext.AddAsync(Room);
                await _appDbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.Success, ex.Message);
            }
        }

        public async Task<GetRoomDto> GetById(Guid id)
        {
            var data = await _appDbContext.RoomMaster.Where(x => x.Id == id && x.IsActive == true).FirstOrDefaultAsync();
            var Room = _mapper.Map<GetRoomDto>(data);
            return Room;
        }

        public async Task<GetRoomDto> GetByName(string name)
        {
            var data = await _appDbContext.RoomMaster.Where(x => x.RoomName == name && x.IsActive == true).FirstOrDefaultAsync();
            var Room = _mapper.Map<GetRoomDto>(data);
            return Room;
        }

        public async Task<List<GetRoomDto>> GetListAsync()
        {
            var data = await _appDbContext.RoomMaster.Where(x => x.IsActive == true).ToListAsync();
            var Room = _mapper.Map<List<GetRoomDto>>(data);
            return Room;
        }

        public async Task<Response> UpdateAsyn(UpdateRoomDto updateRoomDto)
        {
            try
            {
                var data = await _appDbContext.RoomMaster.FindAsync(updateRoomDto.Id);
                var Room = _mapper.Map<RoomMaster>(updateRoomDto);
                data.RoomName = Room.RoomName;
                data.RoomDescription = Room.RoomDescription;
                data.UpdateDate = DateTime.Now;
                data.UpdateBy = Room.UpdateBy;
                data.IsActive = Room.IsActive;

                await _appDbContext.AddAsync(data);
                await _appDbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Exception), CommonMessage.Exception, ex.Message);
            }
        }

        public async Task<bool> UpdateStatus(Guid id, bool status)
        {
            var data = await _appDbContext.RoomMaster.FindAsync(id);
            data.IsActive = status;
            await _appDbContext.AddAsync(data);
            await _appDbContext.SaveChangesAsync();

            return true;
        }
        public async Task<object> CheckDuplicateForEdit(Guid id, string name)
        {
            var data = await _appDbContext.RoomMaster.Where(x => x.Id != id && x.RoomName.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
            var medicine = _mapper.Map<GetRoomDto>(data);
            return medicine;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Rooms
{
    public class UpdateRoomDto
    {
        public Guid Id { get; set; }
        public string? RoomName { get; set; }
        public string? RoomDescription { get; set; }
        public bool IsActive { get; set; }
    }
}

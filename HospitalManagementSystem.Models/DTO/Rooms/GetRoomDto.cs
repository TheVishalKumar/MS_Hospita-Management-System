using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Rooms
{
    public class GetRoomDto : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? RoomName { get; set; }
        public string? RoomDescription { get; set; }
        public bool IsActive { get; set; }
    }
}

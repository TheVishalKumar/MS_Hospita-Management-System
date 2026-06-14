using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Rooms
{
    public class RoomMaster : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? RoomName { get; set; }
        public string? RoomDescription { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; } = 0;
    }

}

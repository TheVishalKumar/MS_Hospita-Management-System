using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class RoomConst
    {
        public static int RoomNameLength = 120;
        public static int DescriptionLength = 500;

        public static string? RoomNameBlankError = "Room Name can not be blank or null";
        public static string? DescriptionBlankError = "Room Description can not be blank or null";
        public static string? RoomNameLengthError = "Room Name accept 120 only.";
        public static string? DescriptionLengthError = "Room Description accept 500 only.";
    }

}

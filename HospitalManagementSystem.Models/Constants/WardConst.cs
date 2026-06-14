using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class WardConst
    {
        public static int WardNameLength = 120;
        public static int DescriptionLength = 500;

        public static string? WardNameBlankError = "Ward Name can not be blank or null";
        public static string? DescriptionBlankError = "Ward Description can not be blank or null";
        public static string? WardNameLengthError = "Ward Name accept 120 only.";
        public static string? DescriptionLengthError = "Ward Description accept 500 only.";
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class DiseaseConsts
    {
        public static int DiseaseNameLength = 120;
        public static int DescriptionLength = 500;

        public static string? DiseaseNameBlankError = "Disease Name can not be blank or null";
        public static string? DescriptionBlankError = "Disease Description can not be blank or null";
        public static string? DiseaseNameLengthError = "Disease Name accept 120 only.";
        public static string? DescriptionLengthError = "Disease Description accept 500 only.";
    }
}

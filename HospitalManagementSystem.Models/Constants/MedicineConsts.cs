using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class MedicineConsts
    {
        public const int MedicineNameMinLength = 2;
        public const int MedicineNameMaxLength = 150;
        public const int MedicineDescriptionMinLength = 10;
        public const int MedicineDescriptionMaxLength = 500;

        public const string MedicineNameLenghtError = "Medicine name should be between in 2 to 150 characters.";
        public const string MedicineDescriptionLenghtError = "Medicine should be between in 10 to 500 characters.";
        
        public const string MedicineNameEmptyError = "Medicine name can not be null or blanck.";
        public const string MedicineDescriptionEmptyError = "Medicine description can not be null or blanck.";

        public const string MedicineHSNCodeEmptyError = "Medicine HSN Code can not be null or blanck.";
        public const string MedicineAmountEmptyError = "Medicine amount can not be null or blanck.";
        public const string MedicineQuantityEmptyError = "Medicine quantity can not be null or blanck.";

    }
}

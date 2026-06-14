using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class DoctorMasterConsts
    {
        public const int UserNameMinLength = 1;
        public const int UserNameMaxLength = 85;
        public const int MinAge = 18;
        public const int MaxAge = 99;
        public const int MobileNoMaxLength = 10;

        public const string FirstNameEmptyError = "First name can not be null or empty.";
        public const string FirstNameLengthError = "First name accept only 85 characters.";

        public const string LastNameEmptyError = "Last name can not be null or empty.";
        public const string LastNameLengthError = "Last name accept only 85 characters.";

        public const string EmailIdCorrectFormat = "Email id should be in correct format.";
        public const string EmailIdEmptyError = "Email id can not be null or empty.";
        public const string MobileNoLengthError = "Mobile no. accept only 10 digits.";
        public const string MobileNoEmptyError = "Mobile no. can not be null or empty.";
        public const string AgeEmptyError = "Age can not be null or blanck.";
        public const string AgeLengthError = "Age should be between in 18 to 99.";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class PatientDetailsConsts
    {
        public const int PatientNameMinLength = 2;
        public const int PatientNameMaxLength = 120;
        public const int MobileNoMaxLength = 10;
        
        public const string FirstNameLengthError = "First name accept only 120 characters.";
        public const string FirstNameEmptyError = "First name can not be blank or null.";
        public const string LastNameLengthError = "Last name accept only 120 characters.";
        public const string LastNameEmptyError = "First name can not be blank or null.";
        public const string EmailIdCorrectFormat = "Email id should be in correct format.";
        public const string MobileNoLengthError = "Mobile no. accept only 10 digits.";
        
    }
}

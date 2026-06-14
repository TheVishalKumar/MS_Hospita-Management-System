using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class UserDetailsConsts
    {
        public const int UserNameMinLength = 2;
        public const int UserNameMaxLength = 65;
        public const int MinAge = 18;
        public const int MaxAge = 60;
        public const int MobileNoMaxLength = 10;
        public const int PasswordLength = 20;


        public const string FirstNameEmptyError = "First name accept only 65 characters.";
        public const string LastNameEmptyError = "Last name accept only 65 characters.";
        public const string EmailIdCorrectFormat = "Email id should be in correct format.";
        public const string MobileNoLengthError = "Mobile no. accept only 10 digits.";
        public const string PasswordLengthError = "Password should be in 20 characters.";
        public const string AgeEmptyError = "Age can not be null or blanck.";
        public const string AgeLengthError = "Age should be between in 18 to 60.";

        public const string UserIdRequiredError = "User id can not be null or empty.";
        public const string PasswordRequiredError = "Password can not be null or empty.";


    }
}

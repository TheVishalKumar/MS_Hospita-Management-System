using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class HospitalBranchConst
    {
        public const int HospitalNameMaxLength = 165;
        public const int DescriptionMaxLength = 500;
        public const int MobileNoMaxLength = 10;
        public const int GSTNoMaxLength = 15;
        public const int FaxNoMaxLength = 10;

        public const string HospitalNameLengthError = "Hospital name accept only 165 characters.";
        public const string DescriptionLengthError = "Hospital description accept only 500 characters.";
        public const string EmailIdCorrectFormat = "Email id should be in correct format.";
        public const string EmailIdBlankError = "Email id can not be null or blank.";

        public const string HospitalNameEmptyError = "Hospital name can not be null or blank.";
        public const string DescriptionEmptyError = "Hospital description can not be null or blank.";
        public const string MobileNoLengthError = "Mobile no. accept only 10 digits.";
        public const string MobileNoBlankError = "Mobile no. can not be null or blank.";

        public const string FaxNoLengthError = "Fax no. accept only 10 digits.";
        public const string FaxNoBlankError = "Fax no. can not be null or blank.";

        public const string GSTNoLengthError = "GST no. accept only 15 digits.";
        public const string GSTNoBlankError = "GST no. can not be null or blank.";
    }
}

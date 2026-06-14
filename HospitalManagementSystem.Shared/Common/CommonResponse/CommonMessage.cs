using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Shared.Common.CommonResponse
{
    public static class CommonMessage
    {
        public const string Success = "Success";
        public const string Failed = "Failed";
        public const string Exception = "Exception";
        public const string DeactiveUser = "Deactive";


        public const string SuccessMessage = "Record saved successfully.";
        public const string UpdateMessage = "Record updated successfully.";
        public const string FailedMessage = "Failed! something is wrong, please try again or connect with admin.";
        public const string ExceptionMessage = "Exception";
        public const string LoginFailed = "Login failed, invalid id or password";
        public const string DeactiveUserFailed = "Login failed, user is deactived. please connect with admin.";
    }
}

using HospitalManagementSystem.Models.DTO.Logins;
using HospitalManagementSystem.Models.DTO.Wards;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Accounts
{
    public interface ILoginRepository
    {
        Task<Response> LoginAsyn(LoginDto loginDto);
        public (bool IsValid, ClaimsPrincipal? Principal, string? Error) ValidateToken(string token);
    }
}

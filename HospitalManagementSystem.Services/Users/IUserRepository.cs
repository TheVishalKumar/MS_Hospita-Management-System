using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Models.Models.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Users
{
    public interface IUserRepository
    {
        Response CreateAsync(CreateUserDetailsDto createUserDetailsDto);
        Response UpdateUser(Guid id, UpdateUserDetailsDto updateUserDetailsDto);
        Task<GetUsersDetailsDto> Get(Guid id);
        Task<List<GetUsersDetailsDto>> GetList();
        Task<List<GetUsersDetailsDto>> GetActiveUserList();
        Task<GetUsersDetailsDto> GetByName(string name);
        Task<GetUsersDetailsDto> GetByMobile(string mobile);
        Task<GetUsersDetailsDto> GetByEmail(string emailId);
        Task<bool> UpdateStatus(Guid id, bool verified, Guid updateBy);

        /// <summary>
        /// Get user by ID (used by RoleAssignmentService and other services)
        /// </summary>
        Task<UserDetails?> GetByIdAsync(Guid userId);

        /// <summary>
        /// Update user (used by RoleAssignmentService and other services)
        /// </summary>
        Task<ApiResponse<UserDetails>> UpdateAsync(UserDetails user);
    }
}

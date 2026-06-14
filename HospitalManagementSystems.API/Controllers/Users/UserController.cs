using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystems.API.Helpers;

namespace HospitalManagementSystems.API.Controllers.Users
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("CreateUser")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateUser(CreateUserDetailsDto createUserDetailsDto)
        {
            try
            {
                var data = await _userRepository.GetByMobile(createUserDetailsDto.MobileNo);
                if (data != null)
                {
                    return this.BadRequestResponse<object>(null, $"User already exist with same '{createUserDetailsDto.MobileNo}' mobile no");
                }

                UserDetailsValidation validations = new UserDetailsValidation();
                var result = validations.Validate(createUserDetailsDto);
                if (!result.IsValid)
                {
                    return this.BadRequestResponse<object>(null, string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                }

                var createResult = _userRepository.CreateAsync(createUserDetailsDto);
                return this.CreatedResponse(createResult, "User created successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error creating user: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateUser/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDetailsDto createUserDetailsDto)
        {
            try
            {
                UpdateUserDetailsValidation validations = new UpdateUserDetailsValidation();
                var result = validations.Validate(createUserDetailsDto);
                if (!result.IsValid)
                {
                    return this.BadRequestResponse<object>(null, string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                }

                var updateResult = _userRepository.UpdateUser(id, createUserDetailsDto);
                return this.OkResponse(updateResult, "User updated successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error updating user: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetUserList")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetUserListAsync()
        {
            try
            {
                var data = await _userRepository.GetList();
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "User list retrieved successfully");
                }
                return this.NotFoundResponse<List<GetUsersDetailsDto>>(null, "No users found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetUsersDetailsDto>>(null, $"Error retrieving user list: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetActiveUserList")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetActiveUserListAsync()
        {
            try
            {
                var data = await _userRepository.GetActiveUserList();
                if (data != null && data.Count > 0)
                {
                    return this.OkResponse(data, "Active user list retrieved successfully");
                }
                return this.NotFoundResponse<List<GetUsersDetailsDto>>(null, "No active users found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetUsersDetailsDto>>(null, $"Error retrieving active user list: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var data = await _userRepository.Get(id);
                if (data != null)
                {
                    return this.OkResponse(data, "User retrieved successfully");
                }
                return this.NotFoundResponse<GetUsersDetailsDto>(null, "User not found");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetUsersDetailsDto>(null, $"Error retrieving user: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("UpdateStatus/{id}/{verified}/{updateBy}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateStatus(Guid id, bool verified, Guid updateBy)
        {
            try
            {
                var result = await _userRepository.UpdateStatus(id, verified, updateBy);
                if (result)
                {
                    return this.OkResponse(result, "User status updated successfully");
                }
                return this.BadRequestResponse<bool>(false, "Failed to update user status");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<bool>(false, $"Error updating user status: {ex.Message}");
            }
        }
    }
}

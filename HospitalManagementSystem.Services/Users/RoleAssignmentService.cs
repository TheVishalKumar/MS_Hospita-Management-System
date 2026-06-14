using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.Models.Users;
using HospitalManagementSystem.Shared.Response;
using System;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Users
{
    /// <summary>
    /// Service for managing user roles and permissions
    /// Only Admin users can assign/modify roles
    /// </summary>
    public interface IRoleAssignmentService
    {
        /// <summary>
        /// Assign role to a user
        /// </summary>
        Task<ApiResponse<UserDetails>> AssignRoleAsync(Guid userId, string role, string assignedByUserId);

        /// <summary>
        /// Get all available roles
        /// </summary>
        Task<ApiResponse<string[]>> GetAllRolesAsync();

        /// <summary>
        /// Check if user has specific role
        /// </summary>
        Task<ApiResponse<bool>> HasRoleAsync(Guid userId, string role);

        /// <summary>
        /// Check if user has any of the specified roles
        /// </summary>
        Task<ApiResponse<bool>> HasAnyRoleAsync(Guid userId, params string[] roles);

        /// <summary>
        /// Validate role is valid before assignment
        /// </summary>
        bool IsValidRole(string role);
    }

    public class RoleAssignmentService : IRoleAssignmentService
    {
        private readonly IUserRepository _userRepository;

        public RoleAssignmentService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Assign role to user (only Admin can do this)
        /// </summary>
        public async Task<ApiResponse<UserDetails>> AssignRoleAsync(Guid userId, string role, string assignedByUserId)
        {
            // Validate role
            if (!IsValidRole(role))
            {
                return ApiResponse<UserDetails>.Failure(
                    "Invalid role",
                    $"Role '{role}' is not valid. Valid roles: {string.Join(", ", UserRoles.AllRoles)}");
            }

            try
            {
                // Get user
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<UserDetails>.Failure(
                        "User not found",
                        $"User with ID {userId} does not exist");
                }

                // Check if assigner is Admin
                var assigner = await _userRepository.GetByIdAsync(Guid.Parse(assignedByUserId));
                if (assigner?.Role != UserRoles.Admin)
                {
                    return ApiResponse<UserDetails>.Failure(
                        "Unauthorized",
                        "Only Admin users can assign roles");
                }

                // Assign role
                var oldRole = user.Role;
                user.Role = role;
                user.UpdateBy = Guid.Parse(assignedByUserId);
                user.UpdateDate = DateTime.UtcNow;

                // Save
                await _userRepository.UpdateAsync(user);

                return ApiResponse<UserDetails>.Success(
                    user,
                    $"Role successfully changed from '{oldRole}' to '{role}' for user {user.FirstName} {user.LastName}");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetails>.Failure(
                    "Error assigning role",
                    ex.Message);
            }
        }

        /// <summary>
        /// Get all available roles
        /// </summary>
        public async Task<ApiResponse<string[]>> GetAllRolesAsync()
        {
            return await Task.FromResult(
                ApiResponse<string[]>.Success(UserRoles.AllRoles, "All available roles"));
        }

        /// <summary>
        /// Check if user has specific role
        /// </summary>
        public async Task<ApiResponse<bool>> HasRoleAsync(Guid userId, string role)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Failure(
                        "User not found",
                        $"User with ID {userId} does not exist");
                }

                var hasRole = user.Role?.Equals(role, StringComparison.OrdinalIgnoreCase) ?? false;
                
                return ApiResponse<bool>.Success(
                    hasRole,
                    $"User has role '{role}': {hasRole}");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(
                    "Error checking role",
                    ex.Message);
            }
        }

        /// <summary>
        /// Check if user has any of the specified roles
        /// </summary>
        public async Task<ApiResponse<bool>> HasAnyRoleAsync(Guid userId, params string[] roles)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Failure(
                        "User not found",
                        $"User with ID {userId} does not exist");
                }

                if (string.IsNullOrWhiteSpace(user.Role))
                {
                    return ApiResponse<bool>.Success(
                        false,
                        "User has no role assigned");
                }

                var hasAnyRole = false;
                foreach (var role in roles)
                {
                    if (user.Role.Equals(role, StringComparison.OrdinalIgnoreCase))
                    {
                        hasAnyRole = true;
                        break;
                    }
                }

                return ApiResponse<bool>.Success(
                    hasAnyRole,
                    $"User has any of roles {string.Join(", ", roles)}: {hasAnyRole}");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(
                    "Error checking roles",
                    ex.Message);
            }
        }

        /// <summary>
        /// Validate role is valid before assignment
        /// </summary>
        public bool IsValidRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return false;

            foreach (var validRole in UserRoles.AllRoles)
            {
                if (validRole.Equals(role, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}

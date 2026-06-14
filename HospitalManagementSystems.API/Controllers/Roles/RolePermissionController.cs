using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Roles;
using HospitalManagementSystem.Services.Roles;
using HospitalManagementSystem.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystems.API.Controllers.Roles
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        /// <summary>
        /// Get all available roles in the system
        /// Can be accessed by any authenticated user
        /// </summary>
        [HttpGet("available-roles")]
        public async Task<ApiResponse<List<string>>> GetAvailableRoles()
        {
            try
            {
                return await _rolePermissionService.GetAllAvailableRolesAsync();
            }
            catch (Exception ex)
            {
                return ApiResponse<List<string>>.Failure("Error retrieving roles", ex.Message);
            }
        }

        /// <summary>
        /// Assign a role to a user (Admin only)
        /// Endpoint: POST /api/rolepermission/assign-role
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("assign-role")]
        public async Task<ApiResponse<RoleAssignmentDto>> AssignRoleToUser([FromBody] AssignRoleDto request)
        {
            if (request == null)
                return ApiResponse<RoleAssignmentDto>.Failure("Invalid request", "Request body is required");

            try
            {
                var adminUserId = GetUserIdFromToken();
                var hospitalId = GetHospitalIdFromToken();
                var branchId = GetBranchIdFromToken();

                return await _rolePermissionService.AssignRoleToUserAsync(
                    request,
                    adminUserId,
                    hospitalId,
                    branchId);
            }
            catch (Exception ex)
            {
                return ApiResponse<RoleAssignmentDto>.Failure("Error assigning role", ex.Message);
            }
        }

        /// <summary>
        /// Get all role assignments for a specific user
        /// Endpoint: GET /api/rolepermission/user/{userId}/roles
        /// </summary>
        [HttpGet("user/{userId}/roles")]
        public async Task<ApiResponse<List<RoleAssignmentDto>>> GetUserRoles(Guid userId)
        {
            if (userId == Guid.Empty)
                return ApiResponse<List<RoleAssignmentDto>>.Failure("Invalid user ID", "User ID is required");

            try
            {
                return await _rolePermissionService.GetUserRolesAsync(userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RoleAssignmentDto>>.Failure("Error retrieving user roles", ex.Message);
            }
        }

        /// <summary>
        /// Get all role assignments in the current hospital/branch (Admin only)
        /// Endpoint: GET /api/rolepermission/all-assignments
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("all-assignments")]
        public async Task<ApiResponse<List<RoleAssignmentDto>>> GetAllRoleAssignments()
        {
            try
            {
                var hospitalId = GetHospitalIdFromToken();
                var branchId = GetBranchIdFromToken();

                return await _rolePermissionService.GetAllRoleAssignmentsAsync(hospitalId, branchId);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RoleAssignmentDto>>.Failure("Error retrieving role assignments", ex.Message);
            }
        }

        /// <summary>
        /// Update a role assignment (Admin only)
        /// Endpoint: PUT /api/rolepermission/update-assignment
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("update-assignment")]
        public async Task<ApiResponse<RoleAssignmentDto>> UpdateRoleAssignment([FromBody] UpdateRoleAssignmentDto request)
        {
            if (request == null)
                return ApiResponse<RoleAssignmentDto>.Failure("Invalid request", "Request body is required");

            try
            {
                var adminUserId = GetUserIdFromToken();
                var hospitalId = GetHospitalIdFromToken();
                var branchId = GetBranchIdFromToken();

                return await _rolePermissionService.UpdateRoleAssignmentAsync(
                    request,
                    adminUserId,
                    hospitalId,
                    branchId);
            }
            catch (Exception ex)
            {
                return ApiResponse<RoleAssignmentDto>.Failure("Error updating role assignment", ex.Message);
            }
        }

        /// <summary>
        /// Remove a role assignment from a user (Admin only)
        /// Endpoint: DELETE /api/rolepermission/remove-role/{roleAssignmentId}
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("remove-role/{roleAssignmentId}")]
        public async Task<ApiResponse<bool>> RemoveRoleFromUser(Guid roleAssignmentId)
        {
            if (roleAssignmentId == Guid.Empty)
                return ApiResponse<bool>.Failure("Invalid ID", "Role assignment ID is required");

            try
            {
                var adminUserId = GetUserIdFromToken();
                return await _rolePermissionService.RemoveRoleFromUserAsync(roleAssignmentId, adminUserId);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("Error removing role", ex.Message);
            }
        }

        /// <summary>
        /// Assign a permission to a role (Admin only)
        /// Endpoint: POST /api/rolepermission/assign-permission
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("assign-permission")]
        public async Task<ApiResponse<RolePermissionDto>> AssignPermissionToRole([FromBody] AssignPermissionDto request)
        {
            if (request == null)
                return ApiResponse<RolePermissionDto>.Failure("Invalid request", "Request body is required");

            try
            {
                var adminUserId = GetUserIdFromToken();
                var hospitalId = GetHospitalIdFromToken();
                var branchId = GetBranchIdFromToken();

                return await _rolePermissionService.AssignPermissionToRoleAsync(
                    request,
                    adminUserId,
                    hospitalId,
                    branchId);
            }
            catch (Exception ex)
            {
                return ApiResponse<RolePermissionDto>.Failure("Error assigning permission", ex.Message);
            }
        }

        /// <summary>
        /// Get all permissions for a role
        /// Endpoint: GET /api/rolepermission/role/{roleName}/permissions
        /// </summary>
        [HttpGet("role/{roleName}/permissions")]
        public async Task<ApiResponse<List<RolePermissionDto>>> GetRolePermissions(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return ApiResponse<List<RolePermissionDto>>.Failure("Invalid role", "Role name is required");

            try
            {
                var hospitalId = GetHospitalIdFromToken();
                var branchId = GetBranchIdFromToken();

                return await _rolePermissionService.GetRolePermissionsAsync(roleName, hospitalId, branchId);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RolePermissionDto>>.Failure("Error retrieving permissions", ex.Message);
            }
        }

        #region Helper Methods

        /// <summary>
        /// Extract user ID from JWT token claims
        /// </summary>
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            throw new UnauthorizedAccessException("User ID not found in token");
        }

        /// <summary>
        /// Extract hospital ID from JWT token claims
        /// </summary>
        private Guid GetHospitalIdFromToken()
        {
            var hospitalIdClaim = User.FindFirst("HospitalId")?.Value;
            if (Guid.TryParse(hospitalIdClaim, out var hospitalId))
                return hospitalId;

            throw new UnauthorizedAccessException("Hospital ID not found in token");
        }

        /// <summary>
        /// Extract branch ID from JWT token claims
        /// </summary>
        private Guid GetBranchIdFromToken()
        {
            var branchIdClaim = User.FindFirst("BranchId")?.Value;
            if (Guid.TryParse(branchIdClaim, out var branchId))
                return branchId;

            throw new UnauthorizedAccessException("Branch ID not found in token");
        }

        #endregion
    }
}

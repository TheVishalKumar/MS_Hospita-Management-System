using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Roles;
using HospitalManagementSystem.Models.Models.Roles;
using HospitalManagementSystem.Models.Models.Users;
using HospitalManagementSystem.Services.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Roles
{
    /// <summary>
    /// Interface for role and permission management
    /// </summary>
    public interface IRolePermissionService
    {
        /// <summary>
        /// Assign a role to a user (Admin only)
        /// </summary>
        Task<ApiResponse<RoleAssignmentDto>> AssignRoleToUserAsync(AssignRoleDto request, Guid adminUserId, Guid hospitalId, Guid branchId);

        /// <summary>
        /// Get all active role assignments for a user
        /// </summary>
        Task<ApiResponse<List<RoleAssignmentDto>>> GetUserRolesAsync(Guid userId);

        /// <summary>
        /// Get all role assignments in a hospital
        /// </summary>
        Task<ApiResponse<List<RoleAssignmentDto>>> GetAllRoleAssignmentsAsync(Guid hospitalId, Guid branchId);

        /// <summary>
        /// Update a role assignment
        /// </summary>
        Task<ApiResponse<RoleAssignmentDto>> UpdateRoleAssignmentAsync(UpdateRoleAssignmentDto request, Guid adminUserId, Guid hospitalId, Guid branchId);

        /// <summary>
        /// Remove/deactivate a role assignment
        /// </summary>
        Task<ApiResponse<bool>> RemoveRoleFromUserAsync(Guid roleAssignmentId, Guid adminUserId);

        /// <summary>
        /// Assign a permission to a role
        /// </summary>
        Task<ApiResponse<RolePermissionDto>> AssignPermissionToRoleAsync(AssignPermissionDto request, Guid adminUserId, Guid hospitalId, Guid branchId);

        /// <summary>
        /// Get all permissions for a role
        /// </summary>
        Task<ApiResponse<List<RolePermissionDto>>> GetRolePermissionsAsync(string roleName, Guid hospitalId, Guid branchId);

        /// <summary>
        /// Check if a user has a specific permission
        /// </summary>
        Task<bool> UserHasPermissionAsync(Guid userId, string permissionName);

        /// <summary>
        /// Get all available roles
        /// </summary>
        Task<ApiResponse<List<string>>> GetAllAvailableRolesAsync();
    }

    /// <summary>
    /// Service for managing roles and permissions
    /// </summary>
    public class RolePermissionService : IRolePermissionService
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        // Valid roles in the system
        private static readonly string[] ValidRoles = new[] 
        { 
            "Admin", "Doctor", "Receptionist", "BillingStaff", 
            "PharmacyStaff", "Employee", "LabTechnician", "RadiologyTechnician" 
        };

        public RolePermissionService(AppDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Assign a role to a user (Admin only)
        /// </summary>
        public async Task<ApiResponse<RoleAssignmentDto>> AssignRoleToUserAsync(
            AssignRoleDto request, 
            Guid adminUserId, 
            Guid hospitalId, 
            Guid branchId)
        {
            try
            {
                // Validate request
                if (request == null)
                    return ApiResponse<RoleAssignmentDto>.Failure("Invalid request", "Request data is required");

                if (request.UserId == Guid.Empty)
                    return ApiResponse<RoleAssignmentDto>.Failure("Invalid user", "User ID is required");

                if (string.IsNullOrWhiteSpace(request.RoleName))
                    return ApiResponse<RoleAssignmentDto>.Failure("Invalid role", "Role name is required");

                // Validate role
                if (!ValidRoles.Contains(request.RoleName))
                    return ApiResponse<RoleAssignmentDto>.Failure(
                        "Invalid role",
                        $"Role '{request.RoleName}' is not valid. Valid roles: {string.Join(", ", ValidRoles)}");

                // Check admin authorization
                var adminUser = await _userRepository.GetByIdAsync(adminUserId);
                if (adminUser == null || adminUser.Role != "Admin")
                    return ApiResponse<RoleAssignmentDto>.Failure(
                        "Unauthorized",
                        "Only Admin users can assign roles");

                // Check if target user exists
                var targetUser = await _userRepository.GetByIdAsync(request.UserId);
                if (targetUser == null)
                    return ApiResponse<RoleAssignmentDto>.Failure("User not found", $"User with ID {request.UserId} does not exist");

                // Check if user already has active role assignment
                var existingAssignment = await _dbContext.UserRoleAssignments
                    .Where(ra => ra.UserId == request.UserId && ra.IsActive && 
                           (ra.ExpiryDate == null || ra.ExpiryDate > DateTime.UtcNow))
                    .FirstOrDefaultAsync();

                if (existingAssignment != null && existingAssignment.RoleName == request.RoleName)
                    return ApiResponse<RoleAssignmentDto>.Failure(
                        "Role already assigned",
                        $"User already has active '{request.RoleName}' role assignment");

                // Create role assignment
                var roleAssignment = new UserRoleAssignment
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    RoleName = request.RoleName,
                    AssignedByUserId = adminUserId,
                    ExpiryDate = request.ExpiryDate,
                    IsActive = true,
                    Remarks = request.Remarks,
                    HospitalId = hospitalId,
                    BranchId = branchId,
                    CreatedDate = DateTime.UtcNow
                };

                // Also update the user's Role field directly
                targetUser.Role = request.RoleName;
                await _userRepository.UpdateAsync(targetUser);

                // Add role assignment record
                _dbContext.UserRoleAssignments.Add(roleAssignment);
                await _dbContext.SaveChangesAsync();

                // Return assigned role
                var adminUserName = $"{adminUser.FirstName} {adminUser.LastName}".Trim();
                var targetUserName = $"{targetUser.FirstName} {targetUser.LastName}".Trim();

                var dto = new RoleAssignmentDto
                {
                    Id = roleAssignment.Id,
                    UserId = roleAssignment.UserId,
                    UserName = targetUserName,
                    UserEmail = targetUser.Email,
                    RoleName = roleAssignment.RoleName,
                    AssignedDate = roleAssignment.AssignedDate,
                    ExpiryDate = roleAssignment.ExpiryDate,
                    IsActive = roleAssignment.IsActive,
                    AssignedByUserName = adminUserName,
                    Remarks = roleAssignment.Remarks,
                    CreatedDate = roleAssignment.CreatedDate
                };

                return ApiResponse<RoleAssignmentDto>.Success(
                    dto,
                    $"Role '{request.RoleName}' successfully assigned to {targetUserName}");
            }
            catch (Exception ex)
            {
                return ApiResponse<RoleAssignmentDto>.Failure(
                    "Error assigning role",
                    ex.Message);
            }
        }

        /// <summary>
        /// Get all active role assignments for a user
        /// </summary>
        public async Task<ApiResponse<List<RoleAssignmentDto>>> GetUserRolesAsync(Guid userId)
        {
            try
            {
                var assignments = await _dbContext.UserRoleAssignments
                    .Where(ra => ra.UserId == userId && ra.IsActive && 
                           (ra.ExpiryDate == null || ra.ExpiryDate > DateTime.UtcNow))
                    .ToListAsync();

                var user = await _userRepository.GetByIdAsync(userId);
                var adminUsers = await _dbContext.UserMaster.ToListAsync();

                var dtos = assignments.Select(ra =>
                {
                    var adminUser = adminUsers.FirstOrDefault(u => u.Id == ra.AssignedByUserId);
                    return new RoleAssignmentDto
                    {
                        Id = ra.Id,
                        UserId = ra.UserId,
                        UserName = $"{user?.FirstName} {user?.LastName}".Trim(),
                        UserEmail = user?.Email,
                        RoleName = ra.RoleName,
                        AssignedDate = ra.AssignedDate,
                        ExpiryDate = ra.ExpiryDate,
                        IsActive = ra.IsActive,
                        AssignedByUserName = $"{adminUser?.FirstName} {adminUser?.LastName}".Trim(),
                        Remarks = ra.Remarks,
                        CreatedDate = ra.CreatedDate
                    };
                }).ToList();

                return ApiResponse<List<RoleAssignmentDto>>.Success(
                    dtos,
                    dtos.Count == 0 ? "No role assignments found" : "Role assignments retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RoleAssignmentDto>>.Failure(
                    "Error retrieving user roles",
                    ex.Message);
            }
        }

        /// <summary>
        /// Get all role assignments in a hospital
        /// </summary>
        public async Task<ApiResponse<List<RoleAssignmentDto>>> GetAllRoleAssignmentsAsync(Guid hospitalId, Guid branchId)
        {
            try
            {
                var assignments = await _dbContext.UserRoleAssignments
                    .Where(ra => ra.HospitalId == hospitalId && ra.BranchId == branchId && ra.IsActive)
                    .ToListAsync();

                var userIds = assignments.Select(ra => ra.UserId).Distinct();
                var users = await _dbContext.UserMaster
                    .Where(u => userIds.Contains(u.Id))
                    .ToListAsync();

                var adminUsers = await _dbContext.UserMaster.ToListAsync();

                var dtos = assignments.Select(ra =>
                {
                    var user = users.FirstOrDefault(u => u.Id == ra.UserId);
                    var adminUser = adminUsers.FirstOrDefault(u => u.Id == ra.AssignedByUserId);
                    return new RoleAssignmentDto
                    {
                        Id = ra.Id,
                        UserId = ra.UserId,
                        UserName = $"{user?.FirstName} {user?.LastName}".Trim(),
                        UserEmail = user?.Email,
                        RoleName = ra.RoleName,
                        AssignedDate = ra.AssignedDate,
                        ExpiryDate = ra.ExpiryDate,
                        IsActive = ra.IsActive,
                        AssignedByUserName = $"{adminUser?.FirstName} {adminUser?.LastName}".Trim(),
                        Remarks = ra.Remarks,
                        CreatedDate = ra.CreatedDate
                    };
                }).ToList();

                return ApiResponse<List<RoleAssignmentDto>>.Success(
                    dtos,
                    dtos.Count == 0 ? "No role assignments found" : "Role assignments retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RoleAssignmentDto>>.Failure(
                    "Error retrieving role assignments",
                    ex.Message);
            }
        }

        /// <summary>
        /// Update a role assignment
        /// </summary>
        public async Task<ApiResponse<RoleAssignmentDto>> UpdateRoleAssignmentAsync(
            UpdateRoleAssignmentDto request,
            Guid adminUserId,
            Guid hospitalId,
            Guid branchId)
        {
            try
            {
                // Check admin authorization
                var adminUser = await _userRepository.GetByIdAsync(adminUserId);
                if (adminUser == null || adminUser.Role != "Admin")
                    return ApiResponse<RoleAssignmentDto>.Failure(
                        "Unauthorized",
                        "Only Admin users can update role assignments");

                // Find role assignment
                var assignment = await _dbContext.UserRoleAssignments
                    .FirstOrDefaultAsync(ra => ra.Id == request.RoleAssignmentId && 
                                              ra.HospitalId == hospitalId && 
                                              ra.BranchId == branchId);

                if (assignment == null)
                    return ApiResponse<RoleAssignmentDto>.Failure(
                        "Role assignment not found",
                        $"Role assignment with ID {request.RoleAssignmentId} does not exist");

                // Update assignment
                assignment.ExpiryDate = request.ExpiryDate;
                assignment.IsActive = request.IsActive;
                assignment.Remarks = request.Remarks;
                assignment.UpdateDate = DateTime.UtcNow;

                _dbContext.UserRoleAssignments.Update(assignment);
                await _dbContext.SaveChangesAsync();

                var user = await _userRepository.GetByIdAsync(assignment.UserId);
                var assignedByUser = await _userRepository.GetByIdAsync(assignment.AssignedByUserId);

                var dto = new RoleAssignmentDto
                {
                    Id = assignment.Id,
                    UserId = assignment.UserId,
                    UserName = $"{user?.FirstName} {user?.LastName}".Trim(),
                    UserEmail = user?.Email,
                    RoleName = assignment.RoleName,
                    AssignedDate = assignment.AssignedDate,
                    ExpiryDate = assignment.ExpiryDate,
                    IsActive = assignment.IsActive,
                    AssignedByUserName = $"{assignedByUser?.FirstName} {assignedByUser?.LastName}".Trim(),
                    Remarks = assignment.Remarks,
                    CreatedDate = assignment.CreatedDate
                };

                return ApiResponse<RoleAssignmentDto>.Success(dto, "Role assignment updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<RoleAssignmentDto>.Failure(
                    "Error updating role assignment",
                    ex.Message);
            }
        }

        /// <summary>
        /// Remove/deactivate a role assignment
        /// </summary>
        public async Task<ApiResponse<bool>> RemoveRoleFromUserAsync(Guid roleAssignmentId, Guid adminUserId)
        {
            try
            {
                // Check admin authorization
                var adminUser = await _userRepository.GetByIdAsync(adminUserId);
                if (adminUser == null || adminUser.Role != "Admin")
                    return ApiResponse<bool>.Failure(
                        "Unauthorized",
                        "Only Admin users can remove roles");

                var assignment = await _dbContext.UserRoleAssignments
                    .FirstOrDefaultAsync(ra => ra.Id == roleAssignmentId);

                if (assignment == null)
                    return ApiResponse<bool>.Failure(
                        "Role assignment not found",
                        $"Role assignment with ID {roleAssignmentId} does not exist");

                assignment.IsActive = false;
                assignment.UpdateDate = DateTime.UtcNow;

                _dbContext.UserRoleAssignments.Update(assignment);
                await _dbContext.SaveChangesAsync();

                return ApiResponse<bool>.Success(true, "Role assignment deactivated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(
                    "Error removing role",
                    ex.Message);
            }
        }

        /// <summary>
        /// Assign a permission to a role
        /// </summary>
        public async Task<ApiResponse<RolePermissionDto>> AssignPermissionToRoleAsync(
            AssignPermissionDto request,
            Guid adminUserId,
            Guid hospitalId,
            Guid branchId)
        {
            try
            {
                // Check admin authorization
                var adminUser = await _userRepository.GetByIdAsync(adminUserId);
                if (adminUser == null || adminUser.Role != "Admin")
                    return ApiResponse<RolePermissionDto>.Failure(
                        "Unauthorized",
                        "Only Admin users can assign permissions");

                // Validate request
                if (string.IsNullOrWhiteSpace(request.RoleName) || string.IsNullOrWhiteSpace(request.PermissionName))
                    return ApiResponse<RolePermissionDto>.Failure(
                        "Invalid request",
                        "Role name and permission name are required");

                // Check if permission already assigned
                var existing = await _dbContext.RolePermissions
                    .FirstOrDefaultAsync(rp => rp.RoleName == request.RoleName && 
                                              rp.PermissionName == request.PermissionName &&
                                              rp.HospitalId == hospitalId &&
                                              rp.BranchId == branchId);

                if (existing != null)
                    return ApiResponse<RolePermissionDto>.Failure(
                        "Permission already assigned",
                        $"Role '{request.RoleName}' already has permission '{request.PermissionName}'");

                // Create permission assignment
                var rolePermission = new RolePermission
                {
                    Id = Guid.NewGuid(),
                    RoleName = request.RoleName,
                    PermissionName = request.PermissionName,
                    PermissionDescription = request.PermissionDescription,
                    IsActive = true,
                    HospitalId = hospitalId,
                    BranchId = branchId,
                    CreatedDate = DateTime.UtcNow
                };

                _dbContext.RolePermissions.Add(rolePermission);
                await _dbContext.SaveChangesAsync();

                var dto = new RolePermissionDto
                {
                    Id = rolePermission.Id,
                    RoleName = rolePermission.RoleName,
                    PermissionName = rolePermission.PermissionName,
                    PermissionDescription = rolePermission.PermissionDescription,
                    IsActive = rolePermission.IsActive,
                    CreatedDate = rolePermission.CreatedDate
                };

                return ApiResponse<RolePermissionDto>.Success(
                    dto,
                    $"Permission '{request.PermissionName}' assigned to role '{request.RoleName}'");
            }
            catch (Exception ex)
            {
                return ApiResponse<RolePermissionDto>.Failure(
                    "Error assigning permission",
                    ex.Message);
            }
        }

        /// <summary>
        /// Get all permissions for a role
        /// </summary>
        public async Task<ApiResponse<List<RolePermissionDto>>> GetRolePermissionsAsync(
            string roleName,
            Guid hospitalId,
            Guid branchId)
        {
            try
            {
                var permissions = await _dbContext.RolePermissions
                    .Where(rp => rp.RoleName == roleName && rp.IsActive &&
                           rp.HospitalId == hospitalId && rp.BranchId == branchId)
                    .ToListAsync();

                var dtos = permissions.Select(rp => new RolePermissionDto
                {
                    Id = rp.Id,
                    RoleName = rp.RoleName,
                    PermissionName = rp.PermissionName,
                    PermissionDescription = rp.PermissionDescription,
                    IsActive = rp.IsActive,
                    CreatedDate = rp.CreatedDate
                }).ToList();

                return ApiResponse<List<RolePermissionDto>>.Success(
                    dtos,
                    dtos.Count == 0 ? "No permissions found for this role" : "Permissions retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RolePermissionDto>>.Failure(
                    "Error retrieving role permissions",
                    ex.Message);
            }
        }

        /// <summary>
        /// Check if a user has a specific permission
        /// </summary>
        public async Task<bool> UserHasPermissionAsync(Guid userId, string permissionName)
        {
            try
            {
                // Get user's active role
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || string.IsNullOrWhiteSpace(user.Role))
                    return false;

                // Check if role has permission
                var hasPermission = await _dbContext.RolePermissions
                    .AnyAsync(rp => rp.RoleName == user.Role && 
                             rp.PermissionName == permissionName && 
                             rp.IsActive);

                return hasPermission;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get all available roles
        /// </summary>
        public async Task<ApiResponse<List<string>>> GetAllAvailableRolesAsync()
        {
            try
            {
                var roles = ValidRoles.ToList();
                return ApiResponse<List<string>>.Success(
                    roles,
                    "Available roles retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<string>>.Failure(
                    "Error retrieving roles",
                    ex.Message);
            }
        }
    }
}

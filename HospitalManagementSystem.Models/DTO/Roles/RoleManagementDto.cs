using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.DTO.Roles
{
    /// <summary>
    /// DTO for assigning a role to a user
    /// Only Admin can perform this operation
    /// </summary>
    public class AssignRoleDto
    {
        /// <summary>
        /// The user ID to assign role to
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The role to assign (Admin, Doctor, Receptionist, BillingStaff, PharmacyStaff, Employee, etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        /// <summary>
        /// Optional expiry date for the role assignment
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Reason or remarks for role assignment
        /// </summary>
        [StringLength(500)]
        public string? Remarks { get; set; }
    }

    /// <summary>
    /// DTO for returning role assignment details
    /// </summary>
    public class RoleAssignmentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public string AssignedByUserName { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// DTO for role permission mapping
    /// </summary>
    public class RolePermissionDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string PermissionName { get; set; }
        public string? PermissionDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// DTO for assigning permission to a role
    /// </summary>
    public class AssignPermissionDto
    {
        /// <summary>
        /// The role to assign permission to
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        /// <summary>
        /// The permission to assign
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PermissionName { get; set; }

        /// <summary>
        /// Description of the permission
        /// </summary>
        [StringLength(500)]
        public string? PermissionDescription { get; set; }
    }

    /// <summary>
    /// DTO for updating role assignment
    /// </summary>
    public class UpdateRoleAssignmentDto
    {
        /// <summary>
        /// The role assignment ID to update
        /// </summary>
        [Required]
        public Guid RoleAssignmentId { get; set; }

        /// <summary>
        /// New expiry date (null to make indefinite)
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Whether to activate or deactivate this role assignment
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Updated remarks
        /// </summary>
        [StringLength(500)]
        public string? Remarks { get; set; }
    }
}

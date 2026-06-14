using HospitalManagementSystem.Shared.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Models.Roles
{
    /// <summary>
    /// Tracks role assignments - which user has which role
    /// Admin uses this to assign roles to employees, receptionist, staff, pharmacy personnel
    /// </summary>
    public class UserRoleAssignment : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The user being assigned a role
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The role being assigned (Admin, Doctor, Receptionist, BillingStaff, PharmacyStaff, Employee, etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        /// <summary>
        /// When the role assignment starts
        /// </summary>
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the role assignment ends (null = active indefinitely)
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Whether this role assignment is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The admin user who assigned this role
        /// </summary>
        [Required]
        public Guid AssignedByUserId { get; set; }

        /// <summary>
        /// Reason for role assignment
        /// </summary>
        [StringLength(500)]
        public string? Remarks { get; set; }

        /// <summary>
        /// Hospital ID for multi-tenant support
        /// </summary>
        public Guid HospitalId { get; set; }

        /// <summary>
        /// Branch ID for multi-tenant support
        /// </summary>
        public Guid BranchId { get; set; }
    }
}

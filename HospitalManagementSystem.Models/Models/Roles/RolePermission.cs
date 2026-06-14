using HospitalManagementSystem.Shared.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Models.Roles
{
    /// <summary>
    /// Maps permissions to roles for granular access control
    /// Admin can manage which permissions are assigned to each role
    /// </summary>
    public class RolePermission : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Role name (Admin, Doctor, Receptionist, BillingStaff, PharmacyStaff, Employee, LabTechnician, RadiologyTechnician)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        /// <summary>
        /// Permission identifier (e.g., patient.create, patient.read, patient.update, patient.delete)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PermissionName { get; set; }

        /// <summary>
        /// Description of what this permission allows
        /// </summary>
        [StringLength(500)]
        public string? PermissionDescription { get; set; }

        /// <summary>
        /// Whether this permission is active/enabled
        /// </summary>
        public bool IsActive { get; set; } = true;

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

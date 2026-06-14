using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Audit
{
    /// <summary>
    /// Records all changes made to entities in the system
    /// Provides complete audit trail for compliance and troubleshooting
    /// </summary>
    public class AuditLog
    {
        /// <summary>
        /// Unique identifier for this audit log entry
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the entity/table that was modified
        /// Example: "DoctorMaster", "PatientDetails", "AppointmentMaster"
        /// </summary>
        public string? EntityName { get; set; }

        /// <summary>
        /// Primary key of the entity that was modified
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Type of action performed: Create, Update, Delete
        /// </summary>
        public string? Action { get; set; } // "Create", "Update", "Delete", "Restore"

        /// <summary>
        /// JSON representation of property values before the change
        /// Null for Create actions
        /// </summary>
        public string? OldValues { get; set; }

        /// <summary>
        /// JSON representation of property values after the change
        /// Null for Delete actions
        /// </summary>
        public string? NewValues { get; set; }

        /// <summary>
        /// User ID who made the change
        /// </summary>
        public Guid ChangedBy { get; set; }

        /// <summary>
        /// Name of the user who made the change (for reference)
        /// </summary>
        public string? ChangedByUserName { get; set; }

        /// <summary>
        /// Timestamp when the change was made (UTC)
        /// </summary>
        public DateTime ChangedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// IP address of the user who made the change
        /// Useful for security audits
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User agent/browser info of the request
        /// Useful for security audits
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Additional context about the change (optional)
        /// Example: reason for deletion, approval status, etc.
        /// </summary>
        public string? Remarks { get; set; }
    }
}

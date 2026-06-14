using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Shared.Common
{
    /// <summary>
    /// Base entity class providing common audit and lifecycle properties
    /// All domain entities should inherit from this class
    /// </summary>
    public class CommonEntity
    {
        /// <summary>
        /// User ID who created this entity
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Timestamp when this entity was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// User ID who last updated this entity
        /// </summary>
        public Guid UpdateBy { get; set; }

        /// <summary>
        /// Timestamp when this entity was last updated
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Indicates if this entity has been soft deleted
        /// Soft delete prevents actual data loss while hiding records
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// User ID who deleted this entity (null if not deleted)
        /// </summary>
        public Guid? DeletedBy { get; set; }

        /// <summary>
        /// Timestamp when this entity was deleted (null if not deleted)
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Version number for optimistic concurrency control
        /// Incremented on each update to prevent lost update scenario
        /// </summary>
        public int Version { get; set; } = 1;
    }

}


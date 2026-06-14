using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Users
{
    /// <summary>
    /// User account entity for system login and access control
    /// Stores user credentials and role information
    /// </summary>
    public class UserDetails : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }

        // Personal Information
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public DateTime DOB { get; set; }
        public string? DOJ { get; set; }

        // Contact Information (Should be encrypted in production)
        public string? Email { get; set; }
        public string? MobileNo { get; set; }

        // Account Information
        public bool? IsActive { get; set; }
        public string? ProfileImage { get; set; }
        
        /// <summary>
        /// Password hash (MUST be hashed with BCrypt, never store plain text)
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// User role for role-based access control
        /// Valid values: Admin, Doctor, Receptionist, Billing_Staff, Pharmacy_Staff, Employee
        /// </summary>
        public string? Role { get; set; }

        // Hospital & Branch Information
        public Guid HospitalId { get; set; }
        public Guid BranchId { get; set; }

        /// <summary>
        /// MFA base32 secret key for TOTP generation (encrypted in database)
        /// Only populated if IsMfaEnabled = true
        /// </summary>
        public string? MfaSecret { get; set; }

        /// <summary>
        /// Indicates if user has completed MFA setup (for Admin accounts)
        /// </summary>
        public bool IsMfaEnabled { get; set; } = false;

        /// <summary>
        /// Recovery codes for MFA account recovery (encrypted in database)
        /// Stored as JSON array: ["CODE1", "CODE2", ...]
        /// </summary>
        public string? MfaRecoveryCodes { get; set; }

        /// <summary>
        /// BackupCodesUsed: Count of recovery codes used (for security tracking)
        /// </summary>
        public int MfaRecoveryCodesUsed { get; set; } = 0;

        /// <summary>
        /// Indicates if user must change password on next login
        /// </summary>
        public bool MustChangePasswordOnNextLogin { get; set; } = true;

        /// <summary>
        /// Last login timestamp for security audit
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Last password change timestamp
        /// </summary>
        public DateTime? LastPasswordChangeDate { get; set; }

        /// <summary>
        /// Account lockout counter (reset after successful login)
        /// </summary>
        public int FailedLoginAttempts { get; set; } = 0;

        /// <summary>
        /// Account locked until this timestamp
        /// </summary>
        public DateTime? LockedUntilDate { get; set; }

        /// <summary>
        /// Two-character country code for telephone number formatting
        /// </summary>
        public string? CountryCodeForPhone { get; set; }

        /// <summary>
        /// Indicates whether user has accepted system terms and conditions
        /// </summary>
        public bool HasAcceptedTerms { get; set; } = false;

        /// <summary>
        /// IP address of last login (for security audit)
        /// </summary>
        public string? LastLoginIpAddress { get; set; }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DBContext.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumnsToUserMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check and add IsDeleted column (part of CommonEntity soft delete)
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserMaster",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Soft delete flag - marks record as deleted without removing from database");

            // Add DeletedBy column (tracks who deleted the user)
            migrationBuilder.AddColumn<System.Guid>(
                name: "DeletedBy",
                table: "UserMaster",
                type: "uniqueidentifier",
                nullable: true,
                comment: "User ID who marked this user as deleted");

            // Add DeletedDate column (tracks when user was deleted)
            migrationBuilder.AddColumn<System.DateTime>(
                name: "DeletedDate",
                table: "UserMaster",
                type: "datetime2",
                nullable: true,
                comment: "Timestamp when user was marked as deleted");

            // MFA Security Columns
            migrationBuilder.AddColumn<string>(
                name: "MfaSecret",
                table: "UserMaster",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Base32 encoded MFA secret for TOTP generation (encrypted)");

            migrationBuilder.AddColumn<bool>(
                name: "IsMfaEnabled",
                table: "UserMaster",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indicates if MFA is enabled for this user");

            migrationBuilder.AddColumn<string>(
                name: "MfaRecoveryCodes",
                table: "UserMaster",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Encrypted JSON array of MFA recovery codes");

            migrationBuilder.AddColumn<int>(
                name: "MfaRecoveryCodesUsed",
                table: "UserMaster",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Count of recovery codes that have been used");

            // Account Lock/Login Attempt Columns
            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "UserMaster",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Count of failed login attempts (reset after successful login)");

            migrationBuilder.AddColumn<System.DateTime>(
                name: "LockedUntilDate",
                table: "UserMaster",
                type: "datetime2",
                nullable: true,
                comment: "Account is locked until this timestamp (security lockout)");

            // Login/Password History Columns
            migrationBuilder.AddColumn<System.DateTime>(
                name: "LastLoginDate",
                table: "UserMaster",
                type: "datetime2",
                nullable: true,
                comment: "Timestamp of the user's last login");

            migrationBuilder.AddColumn<string>(
                name: "LastLoginIpAddress",
                table: "UserMaster",
                type: "nvarchar(50)",
                nullable: true,
                comment: "IP address of the last login (for security audit)");

            migrationBuilder.AddColumn<System.DateTime>(
                name: "LastPasswordChangeDate",
                table: "UserMaster",
                type: "datetime2",
                nullable: true,
                comment: "Timestamp when password was last changed");

            // Password/Terms Columns
            migrationBuilder.AddColumn<bool>(
                name: "MustChangePasswordOnNextLogin",
                table: "UserMaster",
                type: "bit",
                nullable: false,
                defaultValue: true,
                comment: "Indicates if user must change password on next login");

            migrationBuilder.AddColumn<bool>(
                name: "HasAcceptedTerms",
                table: "UserMaster",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indicates if user has accepted terms and conditions");

            // Country/Phone Code Column
            migrationBuilder.AddColumn<string>(
                name: "CountryCodeForPhone",
                table: "UserMaster",
                type: "nvarchar(5)",
                nullable: true,
                comment: "Two-character country code for phone number formatting");

            // Version column for optimistic concurrency
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "UserMaster",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Version number for optimistic concurrency control");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "IsDeleted", table: "UserMaster");
            migrationBuilder.DropColumn(name: "DeletedBy", table: "UserMaster");
            migrationBuilder.DropColumn(name: "DeletedDate", table: "UserMaster");
            migrationBuilder.DropColumn(name: "MfaSecret", table: "UserMaster");
            migrationBuilder.DropColumn(name: "IsMfaEnabled", table: "UserMaster");
            migrationBuilder.DropColumn(name: "MfaRecoveryCodes", table: "UserMaster");
            migrationBuilder.DropColumn(name: "MfaRecoveryCodesUsed", table: "UserMaster");
            migrationBuilder.DropColumn(name: "FailedLoginAttempts", table: "UserMaster");
            migrationBuilder.DropColumn(name: "LockedUntilDate", table: "UserMaster");
            migrationBuilder.DropColumn(name: "LastLoginDate", table: "UserMaster");
            migrationBuilder.DropColumn(name: "LastLoginIpAddress", table: "UserMaster");
            migrationBuilder.DropColumn(name: "LastPasswordChangeDate", table: "UserMaster");
            migrationBuilder.DropColumn(name: "MustChangePasswordOnNextLogin", table: "UserMaster");
            migrationBuilder.DropColumn(name: "HasAcceptedTerms", table: "UserMaster");
            migrationBuilder.DropColumn(name: "CountryCodeForPhone", table: "UserMaster");
            migrationBuilder.DropColumn(name: "Version", table: "UserMaster");
        }
    }
}

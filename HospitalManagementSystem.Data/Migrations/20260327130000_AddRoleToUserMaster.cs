using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DBContext.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToUserMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "UserMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "User role for role-based access control. Valid values: Admin, Doctor, Receptionist, BillingStaff, PharmacyStaff, Employee, LabTechnician, RadiologyTechnician");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserMaster");
        }
    }
}

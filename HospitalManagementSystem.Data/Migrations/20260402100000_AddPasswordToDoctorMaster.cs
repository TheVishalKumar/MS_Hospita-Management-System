using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalManagementSystem.Data.Migrations
{
    public partial class AddPasswordToDoctorMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "DoctorMaster",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Password hash for doctor login (MUST be hashed with BCrypt, never store plain text)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "DoctorMaster");
        }
    }
}

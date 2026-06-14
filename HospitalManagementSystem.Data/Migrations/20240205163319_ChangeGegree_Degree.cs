using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DBContext.Migrations
{
    public partial class ChangeGegree_Degree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gegree_Diplima",
                table: "DoctorEducation",
                newName: "Degree_Diplima");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Degree_Diplima",
                table: "DoctorEducation",
                newName: "Gegree_Diplima");
        }
    }
}

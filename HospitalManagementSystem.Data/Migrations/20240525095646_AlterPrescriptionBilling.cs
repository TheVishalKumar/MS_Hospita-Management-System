using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DBContext.Migrations
{
    public partial class AlterPrescriptionBilling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DueAmount",
                table: "PrescriptionBillings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PaidAmount",
                table: "PrescriptionBillings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "PrescriptionBillings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "PrescriptionBillings");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "PrescriptionBillings");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "PrescriptionBillings");
        }
    }
}

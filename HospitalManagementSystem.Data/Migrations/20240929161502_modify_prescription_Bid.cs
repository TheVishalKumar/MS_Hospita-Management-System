using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DBContext.Migrations
{
    public partial class modify_prescription_Bid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PatientPrescriptionId",
                table: "MedicineMaster",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineMaster_PatientPrescriptionId",
                table: "MedicineMaster",
                column: "PatientPrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineMaster_PatientPrescriptionMaster_PatientPrescriptionId",
                table: "MedicineMaster",
                column: "PatientPrescriptionId",
                principalTable: "PatientPrescriptionMaster",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineMaster_PatientPrescriptionMaster_PatientPrescriptionId",
                table: "MedicineMaster");

            migrationBuilder.DropIndex(
                name: "IX_MedicineMaster_PatientPrescriptionId",
                table: "MedicineMaster");

            migrationBuilder.DropColumn(
                name: "PatientPrescriptionId",
                table: "MedicineMaster");
        }
    }
}

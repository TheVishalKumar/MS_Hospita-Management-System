using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DBContext.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingAuditColumnsToAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add columns to Categories table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Soft delete flag");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                comment: "User who deleted this record");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                comment: "When this record was deleted");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Optimistic concurrency control version");

            // Add columns to DiseaseMaster table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DiseaseMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DiseaseMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "DiseaseMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "DiseaseMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Add columns to DoctorMaster table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DoctorMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DoctorMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "DoctorMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "DoctorMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Add columns to PatientMaster table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PatientMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "PatientMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "PatientMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "PatientMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Add columns to PatientPrescriptionMaster table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PatientPrescriptionMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "PatientPrescriptionMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "PatientPrescriptionMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "PatientPrescriptionMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Add columns to RoomMaster table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RoomMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RoomMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "RoomMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "RoomMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Add columns to WardMaster table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WardMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "WardMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "WardMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "WardMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Add columns to AppointmentMaster table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppointmentMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AppointmentMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "AppointmentMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "AppointmentMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Add columns to PrescriptionBillings table
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PrescriptionBillings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "PrescriptionBillings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "PrescriptionBillings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "PrescriptionBillings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop columns from all tables in Down() method
            var tableNames = new[] { "Categories", "DiseaseMaster", "DoctorMaster", "PatientMaster", "PatientPrescriptionMaster", "RoomMaster", "WardMaster", "AppointmentMaster", "PrescriptionBillings" };
            var columnNames = new[] { "IsDeleted", "DeletedBy", "DeletedDate", "Version" };

            foreach (var table in tableNames)
            {
                foreach (var column in columnNames)
                {
                    migrationBuilder.DropColumn(column, table);
                }
            }
        }
    }
}

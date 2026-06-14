using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DBContext.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumnsToMedicineMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add soft delete columns if they don't exist
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MedicineMaster",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Soft delete flag - indicates if this record is logically deleted");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "MedicineMaster",
                type: "nvarchar(max)",
                nullable: true,
                comment: "User ID or email of who deleted this record");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "MedicineMaster",
                type: "datetime2",
                nullable: true,
                comment: "Timestamp when this record was deleted");

            // Add Version column for optimistic concurrency control
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "MedicineMaster",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Version number for optimistic concurrency control");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MedicineMaster");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "MedicineMaster");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "MedicineMaster");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "MedicineMaster");
        }
    }
}

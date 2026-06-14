using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DBContext.Migrations
{
    public partial class RemoveUnusedEndpoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop DoctorEducation table (let SQL Server handle FK constraints)
            migrationBuilder.DropTable(
                name: "DoctorEducation");

            // Drop Documentation table
            migrationBuilder.DropTable(
                name: "Documentation");

            // Drop DoctorSalary table
            migrationBuilder.DropTable(
                name: "DoctorSalary");

            // Drop PreviousEmployer table
            migrationBuilder.DropTable(
                name: "PreviousEmployer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate DoctorEducation table
            migrationBuilder.CreateTable(
                name: "DoctorEducation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    School_University_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Degree_Diplima = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CourseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GradeAchieved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CoursePeriod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfCompletion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUpto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorEducation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorEducation_DoctorMaster_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorMaster",
                        principalColumn: "Id");
                });

            // Recreate Documentation table
            migrationBuilder.CreateTable(
                name: "Documentation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BackImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentation_DoctorMaster_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorMaster",
                        principalColumn: "Id");
                });

            // Recreate DoctorSalary table
            migrationBuilder.CreateTable(
                name: "DoctorSalary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSalary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorSalary_DoctorMaster_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorMaster",
                        principalColumn: "Id");
                });

            // Recreate PreviousEmployer table
            migrationBuilder.CreateTable(
                name: "PreviousEmployer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousEmployer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousEmployer_DoctorMaster_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorMaster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorEducation_DoctorId",
                table: "DoctorEducation",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentation_DoctorId",
                table: "Documentation",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSalary_DoctorId",
                table: "DoctorSalary",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousEmployer_DoctorId",
                table: "PreviousEmployer",
                column: "DoctorId");
        }
    }
}

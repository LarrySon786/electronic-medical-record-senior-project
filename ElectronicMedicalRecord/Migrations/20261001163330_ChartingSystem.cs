using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ElectronicMedicalRecord.Migrations
{
    /// <inheritdoc />
    public partial class ChartingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChartDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    PractitionerId = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateOnly>(type: "date", nullable: false),
                    SubjectiveNotes = table.Column<string>(type: "text", nullable: false),
                    ObjectiveNotes = table.Column<string>(type: "text", nullable: false),
                    Assesment = table.Column<string>(type: "text", nullable: false),
                    Plan = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChartDb_EmployeeDb_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "EmployeeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChartDb_PatientDb_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PatientDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChartDb_PatientId",
                table: "ChartDb",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartDb_PractitionerId",
                table: "ChartDb",
                column: "PractitionerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChartDb");
        }
    }
}

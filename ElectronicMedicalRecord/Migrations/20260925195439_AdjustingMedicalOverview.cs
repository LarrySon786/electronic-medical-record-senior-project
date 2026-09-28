using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicMedicalRecord.Migrations
{
    /// <inheritdoc />
    public partial class AdjustingMedicalOverview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "MedicalOverviewDb",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "MedicalOverviewDb",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "MedicalOverviewDb",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryHealthcareConcern",
                table: "MedicalOverviewDb",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "MedicalOverviewDb",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "MedicalOverviewDb");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "MedicalOverviewDb");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "MedicalOverviewDb");

            migrationBuilder.DropColumn(
                name: "PrimaryHealthcareConcern",
                table: "MedicalOverviewDb");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "MedicalOverviewDb");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicMedicalRecord.Migrations
{
    /// <inheritdoc />
    public partial class FixMessageEmployeeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageDb_EmployeeId",
                table: "MessageDb");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDb_EmployeeId",
                table: "MessageDb",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageDb_EmployeeId",
                table: "MessageDb");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDb_EmployeeId",
                table: "MessageDb",
                column: "EmployeeId",
                unique: true);
        }
    }
}

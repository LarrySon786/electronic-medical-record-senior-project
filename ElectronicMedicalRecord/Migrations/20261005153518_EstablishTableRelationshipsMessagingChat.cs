using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ElectronicMedicalRecord.Migrations
{
    /// <inheritdoc />
    public partial class EstablishTableRelationshipsMessagingChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParticipantIds = table.Column<List<int>>(type: "integer[]", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatEmployee",
                columns: table => new
                {
                    ChatId = table.Column<int>(type: "integer", nullable: false),
                    ParticipantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatEmployee", x => new { x.ChatId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_ChatEmployee_ChatDb_ChatId",
                        column: x => x.ChatId,
                        principalTable: "ChatDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatEmployee_EmployeeDb_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "EmployeeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SendAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ChatId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageDb_ChatDb_ChatId",
                        column: x => x.ChatId,
                        principalTable: "ChatDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageDb_EmployeeDb_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EmployeeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatEmployee_ParticipantsId",
                table: "ChatEmployee",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDb_ChatId",
                table: "MessageDb",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDb_EmployeeId",
                table: "MessageDb",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatEmployee");

            migrationBuilder.DropTable(
                name: "MessageDb");

            migrationBuilder.DropTable(
                name: "ChatDb");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SangtuariCareerCompass.Migrations
{
    /// <inheritdoc />
    public partial class AddPsychologistId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PsychologistId",
                table: "UserTestResults",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 10, 10, 51, 7, 944, DateTimeKind.Utc).AddTicks(6065), "$2a$11$OowRFjEibhIi0OO1ySbXauaG46MT4ia9MDMMHENA.x0McGCsLyLG2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PsychologistId",
                table: "UserTestResults");

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 9, 8, 49, 43, 194, DateTimeKind.Utc).AddTicks(2267), "$2a$11$UAKnHRcEfgLO8hQOoYryMeNTDFug4f/dLt251Uh76chvgzI4LF1Ii" });
        }
    }
}

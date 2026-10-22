using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SangtuariCareerCompass.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedPsychologist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedPsychologistId",
                table: "UserAssessments",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 10, 22, 7, 55, 29, 336, DateTimeKind.Utc).AddTicks(6537), "$2a$11$bQUNu5BlHZChyDjH4.9vSegIXT2lk9irQjTK5Sy7OGaw/poM845nC" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedPsychologistId",
                table: "UserAssessments");

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 10, 10, 51, 7, 944, DateTimeKind.Utc).AddTicks(6065), "$2a$11$OowRFjEibhIi0OO1ySbXauaG46MT4ia9MDMMHENA.x0McGCsLyLG2" });
        }
    }
}

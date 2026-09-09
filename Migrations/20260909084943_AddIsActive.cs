using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SangtuariCareerCompass.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PsychologistUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "IsActive", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 9, 8, 49, 43, 194, DateTimeKind.Utc).AddTicks(2267), true, "$2a$11$UAKnHRcEfgLO8hQOoYryMeNTDFug4f/dLt251Uh76chvgzI4LF1Ii" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PsychologistUsers");

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 9, 1, 6, 38, 42, 59, DateTimeKind.Utc).AddTicks(3580), "$2a$11$P88R4WPYiO9XusHcuffDOeFDbYX8Iy0HJplFhgvZ.KXuAAypbgdCO" });
        }
    }
}

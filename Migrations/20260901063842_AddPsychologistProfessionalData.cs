using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SangtuariCareerCompass.Migrations
{
    /// <inheritdoc />
    public partial class AddPsychologistProfessionalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "PsychologistUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SilpEndDate",
                table: "PsychologistUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SilpNumber",
                table: "PsychologistUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SilpStartDate",
                table: "PsychologistUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StrNumber",
                table: "PsychologistUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "Degree", "PasswordHash", "SilpEndDate", "SilpNumber", "SilpStartDate", "StrNumber" },
                values: new object[] { new DateTime(2026, 9, 1, 6, 38, 42, 59, DateTimeKind.Utc).AddTicks(3580), null, "$2a$11$P88R4WPYiO9XusHcuffDOeFDbYX8Iy0HJplFhgvZ.KXuAAypbgdCO", null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "PsychologistUsers");

            migrationBuilder.DropColumn(
                name: "SilpEndDate",
                table: "PsychologistUsers");

            migrationBuilder.DropColumn(
                name: "SilpNumber",
                table: "PsychologistUsers");

            migrationBuilder.DropColumn(
                name: "SilpStartDate",
                table: "PsychologistUsers");

            migrationBuilder.DropColumn(
                name: "StrNumber",
                table: "PsychologistUsers");

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 31, 7, 26, 9, 276, DateTimeKind.Utc).AddTicks(5513), "$2a$11$69VBF5IQ5R6aH5alreMiuu8uA4dH3U4GpwqjNoWBldOSqwXTPXDMC" });
        }
    }
}

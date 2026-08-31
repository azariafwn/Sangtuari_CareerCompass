using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SangtuariCareerCompass.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 31, 7, 26, 9, 276, DateTimeKind.Utc).AddTicks(5513), "$2a$11$69VBF5IQ5R6aH5alreMiuu8uA4dH3U4GpwqjNoWBldOSqwXTPXDMC" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 31, 7, 4, 38, 354, DateTimeKind.Utc).AddTicks(5668), "$2a$11$0n.F4Rz9r3eJ7p... (Gunakan Hash BCrypt Asli di sini)" });
        }
    }
}

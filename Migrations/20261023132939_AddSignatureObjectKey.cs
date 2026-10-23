using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SangtuariCareerCompass.Migrations
{
    /// <inheritdoc />
    public partial class AddSignatureObjectKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignatureObjectKey",
                table: "PsychologistUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash", "SignatureObjectKey" },
                values: new object[] { new DateTime(2026, 10, 23, 13, 29, 38, 555, DateTimeKind.Utc).AddTicks(6391), "$2a$11$6MwW1XCAeYuVJ6/tRYPF5uhusf8/HsiWbPVIyMsH5vHgxnxyKlUH.", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureObjectKey",
                table: "PsychologistUsers");

            migrationBuilder.UpdateData(
                table: "PsychologistUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 10, 22, 7, 55, 29, 336, DateTimeKind.Utc).AddTicks(6537), "$2a$11$bQUNu5BlHZChyDjH4.9vSegIXT2lk9irQjTK5Sy7OGaw/poM845nC" });
        }
    }
}

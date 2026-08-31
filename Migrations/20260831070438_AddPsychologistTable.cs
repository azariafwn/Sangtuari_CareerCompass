using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SangtuariCareerCompass.Migrations
{
    /// <inheritdoc />
    public partial class AddPsychologistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PsychologistUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsychologistUsers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PsychologistUsers",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "PasswordHash", "Role" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 8, 31, 7, 4, 38, 354, DateTimeKind.Utc).AddTicks(5668), "admin@sangtuari.com", "Kepala Psikolog Sangtuari", "$2a$11$0n.F4Rz9r3eJ7p... (Gunakan Hash BCrypt Asli di sini)", "Head" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PsychologistUsers");
        }
    }
}

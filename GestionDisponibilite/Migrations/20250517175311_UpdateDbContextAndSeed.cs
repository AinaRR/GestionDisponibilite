using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContextAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "Email", "Nom", "PasswordHash", "Prenom" },
                values: new object[] { "admin@example.com", "Admin", "$argon2id$v=19$m=65536,t=3,p=1$RaDM8stZBY2aRhDcgkcYBQ$v/smiZj8Q85eGkc9650xAVFly2qPTof9g4zSaQkxUZI", "Root" });

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "Email", "Nom", "PasswordHash", "Prenom" },
                values: new object[] { "user@example.com", "User", "$argon2id$v=19$m=65536,t=3,p=1$YGAx9vntoYW473KoYz12og$Fph/ZBr6mtINxdG0pYpriJlsO8ytzJEJiejmkKmhK5M", "Normal" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "Email", "Nom", "PasswordHash", "Prenom" },
                values: new object[] { "", "", "hashed-password-placeholder", "" });

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "Email", "Nom", "PasswordHash", "Prenom" },
                values: new object[] { "", "", "hashed-password-placeholder", "" });
        }
    }
}

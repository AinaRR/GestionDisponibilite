using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePswAdminAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "PasswordHash",
                value: "Admin@2024!");

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "PasswordHash",
                value: "User@2024");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$hPjSlxcsDt2yj3+qvOT5ng$N6Tftu5ZTQk0sqDG6cy0Oqyg3Z3kQZNHA+67gCyL6r4");

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$Jp8o9y3Z8zr1eX1cP5q/aw$zugZ7k6uWU3u4J7kgm6oBlK6S7xGXc3p9ShzO5lPeWE");
        }
    }
}

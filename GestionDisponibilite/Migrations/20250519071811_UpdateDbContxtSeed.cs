using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContxtSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=2,p=1$wphGkXZ...$h1De6k...");

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=2,p=1$yU4n2B...$pAn8eF...");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$ZcE0oy8dHYwbICElw9RRkA$KlHjulvBVkJSdLzDb/p9UFNrp6y4GZFt+djoXmOwkfk");

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$oUItr0YLSB+enmGNoskL8Q$Qg+Uf7Hf6enIQJFpRdWjquO+x43UILqIKdQGpG99fqk");
        }
    }
}

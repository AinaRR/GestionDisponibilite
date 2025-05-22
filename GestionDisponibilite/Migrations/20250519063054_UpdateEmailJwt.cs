using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailJwt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$RaDM8stZBY2aRhDcgkcYBQ$v/smiZj8Q85eGkc9650xAVFly2qPTof9g4zSaQkxUZI");

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$YGAx9vntoYW473KoYz12og$Fph/ZBr6mtINxdG0pYpriJlsO8ytzJEJiejmkKmhK5M");
        }
    }
}

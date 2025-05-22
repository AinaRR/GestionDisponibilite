using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdConstantsChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmployeRoles",
                keyColumns: new[] { "EmployeId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "EmployeRoles",
                keyColumns: new[] { "EmployeId", "RoleId" },
                keyValues: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.InsertData(
                table: "Employes",
                columns: new[] { "EmployeId", "Adresse", "DateDeNaissance", "Degree", "Email", "Nom", "PasswordHash", "Prenom", "Role", "SurProjet", "Telephone", "Username" },
                values: new object[,]
                {
                    { new Guid("4bae40a4-6316-454e-b02e-12683a1cebdd"), "", new DateOnly(1, 1, 1), "", "user@example.com", "User", "$argon2id$v=19$m=65536,t=3,p=1$J+EZTnIFqFnXOs0/HIaFSQ$+T8AE6mc1g4VE1ScQMHOWrw33/RUiIEaC86f/XKv1mw", "Normal", "User", false, "", "normaluser" },
                    { new Guid("5bcd15a0-78d7-4a7e-98c0-731d2bdcdc59"), "", new DateOnly(1, 1, 1), "", "admin@example.com", "Admin", "$argon2id$v=19$m=65536,t=3,p=1$da7YgjEXCLmxQ258xLuyHA$BVTvhrpu8jWJt4H5BYilCaqIPMwIAt4y2xn+44Z6urg", "Root", "Admin", false, "", "adminuser" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("53b4d5dd-ce00-4f79-86de-df5aa32a726f"), "Admin" },
                    { new Guid("65d4adff-ec87-41bd-b521-9243d61ffbb3"), "User" }
                });

            migrationBuilder.InsertData(
                table: "EmployeRoles",
                columns: new[] { "EmployeId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("4bae40a4-6316-454e-b02e-12683a1cebdd"), new Guid("65d4adff-ec87-41bd-b521-9243d61ffbb3") },
                    { new Guid("5bcd15a0-78d7-4a7e-98c0-731d2bdcdc59"), new Guid("53b4d5dd-ce00-4f79-86de-df5aa32a726f") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmployeRoles",
                keyColumns: new[] { "EmployeId", "RoleId" },
                keyValues: new object[] { new Guid("4bae40a4-6316-454e-b02e-12683a1cebdd"), new Guid("65d4adff-ec87-41bd-b521-9243d61ffbb3") });

            migrationBuilder.DeleteData(
                table: "EmployeRoles",
                keyColumns: new[] { "EmployeId", "RoleId" },
                keyValues: new object[] { new Guid("5bcd15a0-78d7-4a7e-98c0-731d2bdcdc59"), new Guid("53b4d5dd-ce00-4f79-86de-df5aa32a726f") });

            migrationBuilder.DeleteData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("4bae40a4-6316-454e-b02e-12683a1cebdd"));

            migrationBuilder.DeleteData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("5bcd15a0-78d7-4a7e-98c0-731d2bdcdc59"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("53b4d5dd-ce00-4f79-86de-df5aa32a726f"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("65d4adff-ec87-41bd-b521-9243d61ffbb3"));

            migrationBuilder.InsertData(
                table: "Employes",
                columns: new[] { "EmployeId", "Adresse", "DateDeNaissance", "Degree", "Email", "Nom", "PasswordHash", "Prenom", "Role", "SurProjet", "Telephone", "Username" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "", new DateOnly(1, 1, 1), "", "admin@example.com", "Admin", "$argon2id$v=19$m=65536,t=3,p=1$da7YgjEXCLmxQ258xLuyHA$BVTvhrpu8jWJt4H5BYilCaqIPMwIAt4y2xn+44Z6urg", "Root", "Admin", false, "", "adminuser" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "", new DateOnly(1, 1, 1), "", "user@example.com", "User", "$argon2id$v=19$m=65536,t=3,p=1$J+EZTnIFqFnXOs0/HIaFSQ$+T8AE6mc1g4VE1ScQMHOWrw33/RUiIEaC86f/XKv1mw", "Normal", "User", false, "", "normaluser" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Admin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "User" }
                });

            migrationBuilder.InsertData(
                table: "EmployeRoles",
                columns: new[] { "EmployeId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("22222222-2222-2222-2222-222222222222") }
                });
        }
    }
}

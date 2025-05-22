using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$oQExXTxSIxHdeKDOPQ8BRw$ErAHwLsb6FQuoHq1Cso9O/6E+sH8I+CElgJ5bSSyJVU");

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$mFAtwBa33jlTRNAiYSOFLw$hYA1SEtIKZbg3VVOVR4Iu8tWohqDJJBgbAJMJCuN5fQ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuthService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$tpG/7isxBJ06i0MVrRbGgA$cha1m6d9Wq4eWmcMGb8RI4AE5z5OzSOvo2sEKp16Ijc");

            migrationBuilder.UpdateData(
                table: "Employes",
                keyColumn: "EmployeId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$WbjTHZkWSgSi0JMlks/Tng$+pvElrz3TDDWH4keEBGYhkGcE51EJyKtAx+DJGL5w+o");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}

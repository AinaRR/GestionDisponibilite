using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employes",
                columns: table => new
                {
                    EmployeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurProjet = table.Column<bool>(type: "bit", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDeNaissance = table.Column<DateOnly>(type: "date", nullable: false),
                    Qualitifactions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employes", x => x.EmployeID);
                });

            migrationBuilder.CreateTable(
                name: "Projets",
                columns: table => new
                {
                    ProjetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomDuProjet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Client = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebutProjet = table.Column<DateOnly>(type: "date", nullable: false),
                    FinProjet = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projets", x => x.ProjetId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeProjets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeProjets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeProjets_Employes_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employes",
                        principalColumn: "EmployeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeProjets_Projets_ProjetId",
                        column: x => x.ProjetId,
                        principalTable: "Projets",
                        principalColumn: "ProjetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeProjets_EmployeId",
                table: "EmployeProjets",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeProjets_ProjetId",
                table: "EmployeProjets",
                column: "ProjetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeProjets");

            migrationBuilder.DropTable(
                name: "Employes");

            migrationBuilder.DropTable(
                name: "Projets");
        }
    }
}

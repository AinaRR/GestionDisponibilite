using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDisponibilite.Migrations
{
    /// <inheritdoc />
    public partial class ChangeQualificationToDegree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Qualitifactions",
                table: "Employes",
                newName: "Degree");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Degree",
                table: "Employes",
                newName: "Qualitifactions");
        }
    }
}

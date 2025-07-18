using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerador_de_testes.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_Materia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materia_Disciplinas_DisciplinaId",
                table: "Materia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materia",
                table: "Materia");

            migrationBuilder.RenameTable(
                name: "Materia",
                newName: "Materias");

            migrationBuilder.RenameIndex(
                name: "IX_Materia_DisciplinaId",
                table: "Materias",
                newName: "IX_Materias_DisciplinaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materias",
                table: "Materias",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materias_Disciplinas_DisciplinaId",
                table: "Materias",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materias_Disciplinas_DisciplinaId",
                table: "Materias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materias",
                table: "Materias");

            migrationBuilder.RenameTable(
                name: "Materias",
                newName: "Materia");

            migrationBuilder.RenameIndex(
                name: "IX_Materias_DisciplinaId",
                table: "Materia",
                newName: "IX_Materia_DisciplinaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materia",
                table: "Materia",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materia_Disciplinas_DisciplinaId",
                table: "Materia",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

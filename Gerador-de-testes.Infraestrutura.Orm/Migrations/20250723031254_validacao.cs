using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerador_de_testes.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class validacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disciplinas_Testes_TesteId",
                table: "Disciplinas");

            migrationBuilder.DropForeignKey(
                name: "FK_Materias_Disciplinas_DisciplinaId",
                table: "Materias");

            migrationBuilder.DropIndex(
                name: "IX_Disciplinas_TesteId",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "TesteId",
                table: "Disciplinas");

            migrationBuilder.CreateTable(
                name: "DisciplinaTeste",
                columns: table => new
                {
                    DisciplinasId = table.Column<Guid>(type: "uuid", nullable: false),
                    TesteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisciplinaTeste", x => new { x.DisciplinasId, x.TesteId });
                    table.ForeignKey(
                        name: "FK_DisciplinaTeste_Disciplinas_DisciplinasId",
                        column: x => x.DisciplinasId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisciplinaTeste_Testes_TesteId",
                        column: x => x.TesteId,
                        principalTable: "Testes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaTeste_TesteId",
                table: "DisciplinaTeste",
                column: "TesteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materias_Disciplinas_DisciplinaId",
                table: "Materias",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materias_Disciplinas_DisciplinaId",
                table: "Materias");

            migrationBuilder.DropTable(
                name: "DisciplinaTeste");

            migrationBuilder.AddColumn<Guid>(
                name: "TesteId",
                table: "Disciplinas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_TesteId",
                table: "Disciplinas",
                column: "TesteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disciplinas_Testes_TesteId",
                table: "Disciplinas",
                column: "TesteId",
                principalTable: "Testes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materias_Disciplinas_DisciplinaId",
                table: "Materias",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerador_de_testes.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_Questoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testes_Disciplinas_DisciplinaId",
                table: "Testes");

            migrationBuilder.DropIndex(
                name: "IX_Testes_DisciplinaId",
                table: "Testes");

            migrationBuilder.DropColumn(
                name: "DisciplinaId",
                table: "Testes");

            migrationBuilder.AddColumn<Guid>(
                name: "TesteId",
                table: "Disciplinas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Questoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Enunciado = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MateriaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questoes_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alternativa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Resposta = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    QuestaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Correta = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alternativa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alternativa_Questoes_QuestaoId",
                        column: x => x.QuestaoId,
                        principalTable: "Questoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestaoTeste",
                columns: table => new
                {
                    QuestoesSelecionadasId = table.Column<Guid>(type: "uuid", nullable: false),
                    TesteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestaoTeste", x => new { x.QuestoesSelecionadasId, x.TesteId });
                    table.ForeignKey(
                        name: "FK_QuestaoTeste_Questoes_QuestoesSelecionadasId",
                        column: x => x.QuestoesSelecionadasId,
                        principalTable: "Questoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestaoTeste_Testes_TesteId",
                        column: x => x.TesteId,
                        principalTable: "Testes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_TesteId",
                table: "Disciplinas",
                column: "TesteId");

            migrationBuilder.CreateIndex(
                name: "IX_Alternativa_QuestaoId",
                table: "Alternativa",
                column: "QuestaoId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestaoTeste_TesteId",
                table: "QuestaoTeste",
                column: "TesteId");

            migrationBuilder.CreateIndex(
                name: "IX_Questoes_MateriaId",
                table: "Questoes",
                column: "MateriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disciplinas_Testes_TesteId",
                table: "Disciplinas",
                column: "TesteId",
                principalTable: "Testes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disciplinas_Testes_TesteId",
                table: "Disciplinas");

            migrationBuilder.DropTable(
                name: "Alternativa");

            migrationBuilder.DropTable(
                name: "QuestaoTeste");

            migrationBuilder.DropTable(
                name: "Questoes");

            migrationBuilder.DropIndex(
                name: "IX_Disciplinas_TesteId",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "TesteId",
                table: "Disciplinas");

            migrationBuilder.AddColumn<Guid>(
                name: "DisciplinaId",
                table: "Testes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Testes_DisciplinaId",
                table: "Testes",
                column: "DisciplinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Testes_Disciplinas_DisciplinaId",
                table: "Testes",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

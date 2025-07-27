using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerador_de_testes.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Config_Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Serie = table.Column<int>(type: "integer", nullable: false),
                    DisciplinaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materias_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Enunciado = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UtilizadaEmTeste = table.Column<bool>(type: "boolean", nullable: false),
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
                name: "Testes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataGeracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Recuperacao = table.Column<bool>(type: "boolean", nullable: false),
                    QuantidadeQuestoes = table.Column<int>(type: "integer", nullable: false),
                    DisciplinaId = table.Column<Guid>(type: "uuid", nullable: false),
                    MateriaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Serie = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Testes_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Testes_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Alternativas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Letra = table.Column<char>(type: "character(1)", nullable: false),
                    Resposta = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Correta = table.Column<bool>(type: "boolean", nullable: false),
                    QuestaoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alternativas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alternativas_Questoes_QuestaoId",
                        column: x => x.QuestaoId,
                        principalTable: "Questoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestaoTeste",
                columns: table => new
                {
                    QuestoesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestaoTeste", x => new { x.QuestoesId, x.TestesId });
                    table.ForeignKey(
                        name: "FK_QuestaoTeste_Questoes_QuestoesId",
                        column: x => x.QuestoesId,
                        principalTable: "Questoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestaoTeste_Testes_TestesId",
                        column: x => x.TestesId,
                        principalTable: "Testes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alternativas_QuestaoId",
                table: "Alternativas",
                column: "QuestaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_DisciplinaId",
                table: "Materias",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestaoTeste_TestesId",
                table: "QuestaoTeste",
                column: "TestesId");

            migrationBuilder.CreateIndex(
                name: "IX_Questoes_MateriaId",
                table: "Questoes",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Testes_DisciplinaId",
                table: "Testes",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Testes_MateriaId",
                table: "Testes",
                column: "MateriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alternativas");

            migrationBuilder.DropTable(
                name: "QuestaoTeste");

            migrationBuilder.DropTable(
                name: "Questoes");

            migrationBuilder.DropTable(
                name: "Testes");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Disciplinas");
        }
    }
}

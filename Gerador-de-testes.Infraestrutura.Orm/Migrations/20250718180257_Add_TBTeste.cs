using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerador_de_testes.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_TBTeste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestesMaterias");

            migrationBuilder.AlterColumn<int>(
                name: "QteQuestoes",
                table: "Testes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MateriaTeste",
                columns: table => new
                {
                    MateriasId = table.Column<Guid>(type: "uuid", nullable: false),
                    TesteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaTeste", x => new { x.MateriasId, x.TesteId });
                    table.ForeignKey(
                        name: "FK_MateriaTeste_Materias_MateriasId",
                        column: x => x.MateriasId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriaTeste_Testes_TesteId",
                        column: x => x.TesteId,
                        principalTable: "Testes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MateriaTeste_TesteId",
                table: "MateriaTeste",
                column: "TesteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MateriaTeste");

            migrationBuilder.AlterColumn<int>(
                name: "QteQuestoes",
                table: "Testes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "TestesMaterias",
                columns: table => new
                {
                    TesteId = table.Column<Guid>(type: "uuid", nullable: false),
                    MateriaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestesMaterias", x => new { x.TesteId, x.MateriaId });
                    table.ForeignKey(
                        name: "FK_TestesMaterias_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestesMaterias_Testes_TesteId",
                        column: x => x.TesteId,
                        principalTable: "Testes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestesMaterias_MateriaId",
                table: "TestesMaterias",
                column: "MateriaId");
        }
    }
}

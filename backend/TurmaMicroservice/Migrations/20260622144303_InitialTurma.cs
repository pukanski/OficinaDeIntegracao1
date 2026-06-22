using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TurmaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialTurma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Turmas",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Ano = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Turno = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turmas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Aluno_Turma",
                columns: table => new
                {
                    AlunoId = table.Column<long>(type: "bigint", nullable: false),
                    TurmaId = table.Column<long>(type: "bigint", nullable: false),
                    MatriculadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aluno_Turma", x => new { x.AlunoId, x.TurmaId });
                    table.ForeignKey(
                        name: "FK_Aluno_Turma_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Professor_Turma",
                columns: table => new
                {
                    ProfessorId = table.Column<long>(type: "bigint", nullable: false),
                    TurmaId = table.Column<long>(type: "bigint", nullable: false),
                    VinculadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professor_Turma", x => new { x.ProfessorId, x.TurmaId });
                    table.ForeignKey(
                        name: "FK_Professor_Turma_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aluno_Turma_TurmaId",
                table: "Aluno_Turma",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Professor_Turma_TurmaId",
                table: "Professor_Turma",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_Nome_Ano_Turno",
                table: "Turmas",
                columns: new[] { "Nome", "Ano", "Turno" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aluno_Turma");

            migrationBuilder.DropTable(
                name: "Professor_Turma");

            migrationBuilder.DropTable(
                name: "Turmas");
        }
    }
}

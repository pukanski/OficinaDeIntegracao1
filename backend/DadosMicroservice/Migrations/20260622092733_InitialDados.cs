using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DadosAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resposta_Aluno",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlunoId = table.Column<long>(type: "bigint", nullable: false),
                    QuestaoId = table.Column<long>(type: "bigint", nullable: false),
                    AlternativaId = table.Column<long>(type: "bigint", nullable: false),
                    Disciplina = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Vestibular = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    AnoProva = table.Column<int>(type: "integer", nullable: false),
                    Dificuldade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Acertou = table.Column<bool>(type: "boolean", nullable: false),
                    RespondidoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resposta_Aluno", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resposta_Aluno_AlunoId",
                table: "Resposta_Aluno",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Resposta_Aluno_AlunoId_QuestaoId",
                table: "Resposta_Aluno",
                columns: new[] { "AlunoId", "QuestaoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resposta_Aluno_Disciplina",
                table: "Resposta_Aluno",
                column: "Disciplina");

            migrationBuilder.CreateIndex(
                name: "IX_Resposta_Aluno_RespondidoEm",
                table: "Resposta_Aluno",
                column: "RespondidoEm");

            migrationBuilder.CreateIndex(
                name: "IX_Resposta_Aluno_Vestibular",
                table: "Resposta_Aluno",
                column: "Vestibular");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resposta_Aluno");
        }
    }
}

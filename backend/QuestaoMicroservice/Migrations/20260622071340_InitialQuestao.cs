using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuestaoAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialQuestao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provas",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Vestibular = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    Edicao = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Questoes",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProvaId = table.Column<long>(type: "bigint", nullable: false),
                    Disciplina = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Materia = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Enunciado = table.Column<string>(type: "text", nullable: false),
                    Dificuldade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questoes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Questoes_Provas_ProvaId",
                        column: x => x.ProvaId,
                        principalTable: "Provas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alternativas",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestaoId = table.Column<long>(type: "bigint", nullable: false),
                    Letra = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    Texto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Correta = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alternativas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Alternativas_Questoes_QuestaoId",
                        column: x => x.QuestaoId,
                        principalTable: "Questoes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alternativas_QuestaoId_Letra",
                table: "Alternativas",
                columns: new[] { "QuestaoId", "Letra" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provas_Vestibular_Ano_Edicao",
                table: "Provas",
                columns: new[] { "Vestibular", "Ano", "Edicao" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questoes_ProvaId_Numero",
                table: "Questoes",
                columns: new[] { "ProvaId", "Numero" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alternativas");

            migrationBuilder.DropTable(
                name: "Questoes");

            migrationBuilder.DropTable(
                name: "Provas");
        }
    }
}

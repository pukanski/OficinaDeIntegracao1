using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ListaMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class InitialLista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Listas",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ProfessorId = table.Column<long>(type: "bigint", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Lista_Questao",
                columns: table => new
                {
                    ListaId = table.Column<long>(type: "bigint", nullable: false),
                    QuestaoId = table.Column<long>(type: "bigint", nullable: false),
                    AdicionadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lista_Questao", x => new { x.ListaId, x.QuestaoId });
                    table.ForeignKey(
                        name: "FK_Lista_Questao_Listas_ListaId",
                        column: x => x.ListaId,
                        principalTable: "Listas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lista_Turma",
                columns: table => new
                {
                    ListaId = table.Column<long>(type: "bigint", nullable: false),
                    TurmaId = table.Column<long>(type: "bigint", nullable: false),
                    AtribuidoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lista_Turma", x => new { x.ListaId, x.TurmaId });
                    table.ForeignKey(
                        name: "FK_Lista_Turma_Listas_ListaId",
                        column: x => x.ListaId,
                        principalTable: "Listas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lista_Questao");

            migrationBuilder.DropTable(
                name: "Lista_Turma");

            migrationBuilder.DropTable(
                name: "Listas");
        }
    }
}

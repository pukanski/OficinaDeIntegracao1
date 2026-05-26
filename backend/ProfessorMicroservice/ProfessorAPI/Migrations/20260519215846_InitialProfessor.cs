using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProfessorAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialProfessor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Professores",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    siape = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    primeiroNome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    ultimoNome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    disciplina = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professores", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professores_email",
                table: "Professores",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professores_siape",
                table: "Professores",
                column: "siape",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Professores");
        }
    }
}

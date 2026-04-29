using AlunoAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AlunoAPI.Data
{
    public class AppDbContext : DbContext
    {
        // DbContextOptions carrega as configs de conexão (string de conexão, provider, etc.)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Representa a tabela "Aluno" no banco
        public DbSet<Aluno> Alunos { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using DadosAPI.Model;

namespace DadosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RespostaAluno> RespostasAlunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Impede que o mesmo aluno responda a mesma questão duas vezes
            modelBuilder.Entity<RespostaAluno>()
                .HasIndex(r => new { r.AlunoId, r.QuestaoId })
                .IsUnique();

            // Índices para otimizar as queries do dashboard
            modelBuilder.Entity<RespostaAluno>()
                .HasIndex(r => r.AlunoId);

            modelBuilder.Entity<RespostaAluno>()
                .HasIndex(r => r.Disciplina);

            modelBuilder.Entity<RespostaAluno>()
                .HasIndex(r => r.Vestibular);

            modelBuilder.Entity<RespostaAluno>()
                .HasIndex(r => r.RespondidoEm);
        }
    }
}

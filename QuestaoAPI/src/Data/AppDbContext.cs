using Microsoft.EntityFrameworkCore;
using QuestaoAPI.Model;

namespace QuestaoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Prova> Provas { get; set; }
        public DbSet<Questao> Questoes { get; set; }
        public DbSet<Alternativa> Alternativas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── Prova ────────────────────────────────────────────────────────
            // Unicidade: mesma prova não pode ser cadastrada duas vezes
            modelBuilder.Entity<Prova>()
                .HasIndex(p => new { p.Vestibular, p.Ano, p.Edicao })
                .IsUnique();

            // ── Questao ──────────────────────────────────────────────────────
            modelBuilder.Entity<Questao>()
                .HasOne(q => q.Prova)
                .WithMany(p => p.Questoes)
                .HasForeignKey(q => q.ProvaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unicidade: mesma questão (número) não pode aparecer duas vezes na mesma prova
            modelBuilder.Entity<Questao>()
                .HasIndex(q => new { q.ProvaId, q.Numero })
                .IsUnique();

            // ── Alternativa ───────────────────────────────────────────────────
            modelBuilder.Entity<Alternativa>()
                .HasOne(a => a.Questao)
                .WithMany(q => q.Alternativas)
                .HasForeignKey(a => a.QuestaoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unicidade: mesma letra não pode aparecer duas vezes na mesma questão
            modelBuilder.Entity<Alternativa>()
                .HasIndex(a => new { a.QuestaoId, a.Letra })
                .IsUnique();
        }
    }
}

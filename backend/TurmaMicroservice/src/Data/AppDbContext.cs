using Microsoft.EntityFrameworkCore;
using TurmaAPI.Model;

namespace TurmaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Turma> Turmas { get; set; }
        public DbSet<AlunoTurma> AlunoTurmas { get; set; }
        public DbSet<ProfessorTurma> ProfessorTurmas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── AlunoTurma ──────────────────────────────────────────────────
            modelBuilder.Entity<AlunoTurma>()
                .HasKey(at => new { at.AlunoId, at.TurmaId });

            modelBuilder.Entity<AlunoTurma>()
                .HasOne(at => at.Turma)
                .WithMany()
                .HasForeignKey(at => at.TurmaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── ProfessorTurma ───────────────────────────────────────────────
            modelBuilder.Entity<ProfessorTurma>()
                .HasKey(pt => new { pt.ProfessorId, pt.TurmaId });

            modelBuilder.Entity<ProfessorTurma>()
                .HasOne(pt => pt.Turma)
                .WithMany()
                .HasForeignKey(pt => pt.TurmaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Turma — unicidade de nome + ano + turno ──────────────────────
            modelBuilder.Entity<Turma>()
                .HasIndex(t => new { t.Nome, t.Ano, t.Turno })
                .IsUnique();
        }
    }
}

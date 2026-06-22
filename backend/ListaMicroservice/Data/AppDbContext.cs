using Microsoft.EntityFrameworkCore;
using ListaAPI.Model;

namespace ListaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Lista> Listas { get; set; }
        public DbSet<ListaQuestao> ListaQuestoes { get; set; }
        public DbSet<ListaTurma> ListaTurmas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListaQuestao>()
                .HasKey(lq => new { lq.ListaId, lq.QuestaoId });

            modelBuilder.Entity<ListaQuestao>()
                .HasOne<Lista>()
                .WithMany()
                .HasForeignKey(lq => lq.ListaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ListaTurma>()
                .HasKey(lt => new { lt.ListaId, lt.TurmaId });

            modelBuilder.Entity<ListaTurma>()
                .HasOne<Lista>()
                .WithMany()
                .HasForeignKey(lt => lt.ListaId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
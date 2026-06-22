using Microsoft.EntityFrameworkCore;
using ProfessorAPI.src.Model;

namespace ProfessorAPI.src.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Professor> Professores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Garante unicidade de SIAPE e Email no banco
            modelBuilder.Entity<Professor>()
                .HasIndex(p => p.Siape)
                .IsUnique();

            modelBuilder.Entity<Professor>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }

}


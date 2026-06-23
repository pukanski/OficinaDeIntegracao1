using Microsoft.EntityFrameworkCore;
using ProfessorAPI.src.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfessorAPI.src.Data
{
    // Modelo local de Aluno para o AdminController poder inserir na tabela Aluno
    [Table("Aluno")]
    public class AlunoEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("AuthId")]
        public Guid AuthId { get; set; }

        [Required]
        [MaxLength(80)]
        public string PrimeiroNome { get; set; } = "";

        [Required]
        [MaxLength(80)]
        public string UltimoNome { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string RA { get; set; } = "";

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Professor> Professores { get; set; }
        public DbSet<AlunoEntity> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Professor>()
                .HasIndex(p => p.Siape)
                .IsUnique();

            modelBuilder.Entity<Professor>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<AlunoEntity>()
                .HasIndex(a => a.Email)
                .IsUnique();
        }
    }
}


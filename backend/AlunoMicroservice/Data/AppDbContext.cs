using AlunoAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AlunoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Frequencia> Frequencias { get; set; }
    }
}

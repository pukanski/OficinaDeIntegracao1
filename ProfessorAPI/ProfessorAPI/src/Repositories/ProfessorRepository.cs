using Microsoft.EntityFrameworkCore;
using ProfessorAPI.src.Data;
using ProfessorAPI.src.Model;

namespace ProfessorAPI.src.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly AppDbContext _context;
        public ProfessorRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<Professor> CreateProfessorAsync(Professor professor)
        {
            _context.Professores.Add(professor);
            await _context.SaveChangesAsync();
            return new Professor();
        }

        public async Task<bool> DeleteProfessorAsync(long id)
        {
            var professor = await _context.Professores.FindAsync(id);
            if (professor == null) return false;

            _context.Professores.Remove(professor);
            await _context.SaveChangesAsync(); 
            return true;

        }

        public async Task<bool> ExisteEmailAsync(string email, long? ignorarId = null) =>
            await _context.Professores
                .AnyAsync(p => p.email == email && (ignorarId == null || p.id != ignorarId));

        public async Task<bool> ExisteSiapeAsync(string siape, long? ignorarId = null) =>
            await _context.Professores
                .AnyAsync(p => p.siape == siape && (ignorarId == null || p.id != ignorarId));

        public async Task<List<Professor>> GetAllProfessoresAsync() =>
            await _context.Professores.ToListAsync();

        public async Task<Professor?> GetProfessorByIdAsync(long id) =>
            await _context.Professores.FindAsync(id);

        public async Task<Professor?> UpdateProfessorAsync(long id, Professor professorAtualizadoDados)
        {
            var professor = await _context.Professores.FindAsync(id);
            if(professor == null) return null;

            professor.siape = professorAtualizadoDados.siape;
            professor.primeiroNome = professorAtualizadoDados.primeiroNome;
            professor.ultimoNome = professorAtualizadoDados.ultimoNome;
            professor.email = professorAtualizadoDados.email;
            professor.disciplina = professorAtualizadoDados.disciplina;
            professor.AtualizadoEm = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return professor;
        }
    }
}

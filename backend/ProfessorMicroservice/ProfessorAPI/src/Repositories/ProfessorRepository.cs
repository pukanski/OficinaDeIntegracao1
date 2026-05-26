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
            return professor;
        }

        public async Task<bool> DeleteProfessorAsync(long id)
        {
            var professor = await _context.Professores.FindAsync(id);

             if (professor == null)
                return false;

            _context.Professores.Remove(professor);
            await _context.SaveChangesAsync(); 
            return true;

        }

        /*public async Task<bool> ExisteEmailAsync(string email, long? ignorarId = null)
        {
            return await _context.Professores.AnyAsync(p => p.Email == email);
        }

        public async Task<bool> ExisteSiapeAsync(string siape, long? ignorarId = null)
        {
            return await _context.Professores.AnyAsync(p => p.Siape == siape);
        }*/

        public async Task<List<Professor>> GetAllProfessoresAsync() =>
            await _context.Professores.ToListAsync();

        public async Task<Professor?> GetProfessorByIdAsync(long id) =>
            await _context.Professores.FindAsync(id);

        public async Task<Professor?> UpdateProfessorAsync(long id, Professor professorAtualizadoDados)
        {
            var professor = await _context.Professores.FindAsync(id);
            if(professor == null) return null;

            professor.Siape = professorAtualizadoDados.Siape;
            professor.PrimeiroNome = professorAtualizadoDados.PrimeiroNome;
            professor.UltimoNome = professorAtualizadoDados.UltimoNome;
            professor.Email = professorAtualizadoDados.Email;
            professor.Disciplina = professorAtualizadoDados.Disciplina;
            professor.AtualizadoEm = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return professor;
        }
    }
}

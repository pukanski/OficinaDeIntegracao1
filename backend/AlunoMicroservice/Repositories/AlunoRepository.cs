using AlunoAPI.Data;
using AlunoAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AlunoAPI.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {

        private readonly AppDbContext _context;

        public AlunoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Aluno> CreateAlunoAsync(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync(); // persiste no banco
            return aluno; // aluno.Id já estará preenchido após o SaveChanges

        }

        public async Task<List<Aluno>> GetAllAlunosAsync()
        {
            return await _context.Alunos.ToListAsync();

        }

        public async Task<Aluno?> GetAlunoByIdAsync(long id)
        {

            return await _context.Alunos.FindAsync(id);

        }

        public async Task<Aluno?> UpdateAlunoByIdAsync(long id, Aluno novoAluno)
        {
            var alunoExistente = await _context.Alunos.FindAsync(id);
            if (alunoExistente == null)
            {
                return null;
            }

            alunoExistente.Id = id; // garante que o ID do aluno atualizado seja o mesmo
            alunoExistente.PrimeiroNome = novoAluno.PrimeiroNome;
            alunoExistente.UltimoNome = novoAluno.UltimoNome;
            alunoExistente.Email = novoAluno.Email;
            alunoExistente.RA = novoAluno.RA;

            await _context.SaveChangesAsync();
            return alunoExistente;
        }

        public async Task<bool> DeleteAlunoByIdAsync(long id)
        {
            var alunoExistente = await _context.Alunos.FindAsync(id);
            if (alunoExistente == null)
            {
                return false;
            }
            _context.Alunos.Remove(alunoExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


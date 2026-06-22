using Microsoft.EntityFrameworkCore;
using TurmaAPI.Data;
using TurmaAPI.Model;

namespace TurmaAPI.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly AppDbContext _context;

        public TurmaRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── CRUD ─────────────────────────────────────────────────────────────

        public async Task<Turma> CreateAsync(Turma turma)
        {
            _context.Turmas.Add(turma);
            await _context.SaveChangesAsync();
            return turma;
        }

        public async Task<List<(Turma turma, int qtdAlunos, int qtdProfessores)>> GetAllAsync()
        {
            var turmas = await _context.Turmas.ToListAsync();

            var resultado = new List<(Turma, int, int)>();
            foreach (var turma in turmas)
            {
                var qtdAlunos = await ContarAlunosAsync(turma.Id);
                var qtdProfessores = await ContarProfessoresAsync(turma.Id);
                resultado.Add((turma, qtdAlunos, qtdProfessores));
            }

            return resultado;
        }

        public async Task<Turma?> GetByIdAsync(long id) =>
            await _context.Turmas.FindAsync(id);

        public async Task<Turma?> UpdateAsync(long id, Turma dadosNovos)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null) return null;

            turma.Nome = dadosNovos.Nome;
            turma.Ano = dadosNovos.Ano;
            turma.Turno = dadosNovos.Turno;
            turma.AtualizadoEm = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return turma;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null) return false;

            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Validação ─────────────────────────────────────────────────────────

        public async Task<bool> ExisteTurmaAsync(string nome, string ano, string turno, long? ignorarId = null) =>
            await _context.Turmas.AnyAsync(t =>
                t.Nome == nome &&
                t.Ano == ano &&
                t.Turno == turno &&
                (ignorarId == null || t.Id != ignorarId));

        // ── Aluno_Turma ───────────────────────────────────────────────────────

        public async Task<bool> AlunoJaMatriculadoAsync(long alunoId, long turmaId) =>
            await _context.AlunoTurmas
                .AnyAsync(at => at.AlunoId == alunoId && at.TurmaId == turmaId);

        public async Task MatricularAlunoAsync(long alunoId, long turmaId)
        {
            var vinculo = new AlunoTurma
            {
                AlunoId = alunoId,
                TurmaId = turmaId,
                MatriculadoEm = DateTime.UtcNow
            };
            _context.AlunoTurmas.Add(vinculo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DesmatricularAlunoAsync(long alunoId, long turmaId)
        {
            var vinculo = await _context.AlunoTurmas
                .FirstOrDefaultAsync(at => at.AlunoId == alunoId && at.TurmaId == turmaId);

            if (vinculo == null) return false;

            _context.AlunoTurmas.Remove(vinculo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ContarAlunosAsync(long turmaId) =>
            await _context.AlunoTurmas.CountAsync(at => at.TurmaId == turmaId);

        public async Task<List<long>> GetAlunosIdsDaTurmaAsync(long turmaId)
        {
            return await _context.AlunoTurmas
                .Where(at => at.TurmaId == turmaId)
                .Select(at => at.AlunoId)
                .ToListAsync();
        }

        // ── Professor_Turma ───────────────────────────────────────────────────

        public async Task<bool> ProfessorJaVinculadoAsync(long professorId, long turmaId) =>
            await _context.ProfessorTurmas
                .AnyAsync(pt => pt.ProfessorId == professorId && pt.TurmaId == turmaId);

        public async Task VincularProfessorAsync(long professorId, long turmaId)
        {
            var vinculo = new ProfessorTurma
            {
                ProfessorId = professorId,
                TurmaId = turmaId,
                VinculadoEm = DateTime.UtcNow
            };
            _context.ProfessorTurmas.Add(vinculo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DesvincularProfessorAsync(long professorId, long turmaId)
        {
            var vinculo = await _context.ProfessorTurmas
                .FirstOrDefaultAsync(pt => pt.ProfessorId == professorId && pt.TurmaId == turmaId);

            if (vinculo == null) return false;

            _context.ProfessorTurmas.Remove(vinculo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ContarProfessoresAsync(long turmaId) =>
            await _context.ProfessorTurmas.CountAsync(pt => pt.TurmaId == turmaId);

        public async Task<List<long>> GetProfessoresIdsDaTurmaAsync(long turmaId)
        {
            return await _context.ProfessorTurmas
                .Where(pt => pt.TurmaId == turmaId)
                .Select(pt => pt.ProfessorId)
                .ToListAsync();
        }
    }
}

using TurmaAPI.Model;

namespace TurmaAPI.Repositories
{
    public interface ITurmaRepository
    {
        // ── CRUD ────────────────────────────────────────────────────────────
        Task<Turma> CreateAsync(Turma turma);
        Task<List<(Turma turma, int qtdAlunos, int qtdProfessores)>> GetAllAsync();
        Task<Turma?> GetByIdAsync(long id);
        Task<Turma?> UpdateAsync(long id, Turma turma);
        Task<bool> DeleteAsync(long id);

        // ── Validação ────────────────────────────────────────────────────────
        Task<bool> ExisteTurmaAsync(string nome, string ano, string turno, long? ignorarId = null);

        // ── Aluno_Turma ──────────────────────────────────────────────────────
        Task<bool> AlunoJaMatriculadoAsync(long alunoId, long turmaId);
        Task MatricularAlunoAsync(long alunoId, long turmaId);
        Task<bool> DesmatricularAlunoAsync(long alunoId, long turmaId);
        Task<int> ContarAlunosAsync(long turmaId);
        Task<List<long>> GetAlunosIdsDaTurmaAsync(long turmaId);


        // ── Professor_Turma ──────────────────────────────────────────────────
        Task<bool> ProfessorJaVinculadoAsync(long professorId, long turmaId);
        Task VincularProfessorAsync(long professorId, long turmaId);
        Task<bool> DesvincularProfessorAsync(long professorId, long turmaId);
        Task<int> ContarProfessoresAsync(long turmaId);
        Task<List<long>> GetProfessoresIdsDaTurmaAsync(long turmaId);
    }
}

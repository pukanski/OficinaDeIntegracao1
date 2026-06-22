using TurmaAPI.DTOs;

namespace TurmaAPI.Services
{
    public interface ITurmaService
    {
        // ── CRUD ────────────────────────────────────────────────────────────
        Task<TurmaResponseDTO> CreateAsync(TurmaRequestDTO dto);
        Task<List<TurmaResponseDTO>> GetAllAsync();
        Task<TurmaResponseDTO> GetByIdAsync(long id);
        Task<TurmaResponseDTO> UpdateAsync(long id, TurmaRequestDTO dto);
        Task<bool> DeleteAsync(long id);

        // ── Aluno_Turma ──────────────────────────────────────────────────────
        Task MatricularAlunoAsync(long turmaId, long alunoId);
        Task DesmatricularAlunoAsync(long turmaId, long alunoId);

        // ── Professor_Turma ──────────────────────────────────────────────────
        Task VincularProfessorAsync(long turmaId, long professorId);
        Task DesvincularProfessorAsync(long turmaId, long professorId);
    }
}

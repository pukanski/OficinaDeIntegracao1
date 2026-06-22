using TurmaAPI.DTOs;
using TurmaAPI.Mappers;
using TurmaAPI.Repositories;

namespace TurmaAPI.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _repository;

        public TurmaService(ITurmaRepository repository)
        {
            _repository = repository;
        }

        // ── CRUD ─────────────────────────────────────────────────────────────

        public async Task<TurmaResponseDTO> CreateAsync(TurmaRequestDTO dto)
        {
            if (await _repository.ExisteTurmaAsync(dto.Nome, dto.Ano, dto.Turno))
                throw new InvalidOperationException("Já existe uma turma com esse nome, ano e turno.");

            var turma = TurmaMapper.ToModel(dto);
            var criada = await _repository.CreateAsync(turma);
            return TurmaMapper.ToResponse(criada);
        }

        public async Task<List<TurmaResponseDTO>> GetAllAsync()
        {
            var turmas = await _repository.GetAllAsync();
            return TurmaMapper.ToResponseList(turmas);
        }

        public async Task<TurmaResponseDTO> GetByIdAsync(long id)
        {
            var turma = await _repository.GetByIdAsync(id);
            if (turma == null)
                throw new KeyNotFoundException($"Turma com ID {id} não encontrada.");

            var qtdAlunos = await _repository.ContarAlunosAsync(id);
            var qtdProfessores = await _repository.ContarProfessoresAsync(id);

            var alunosIds = await _repository.GetAlunosIdsDaTurmaAsync(id);
            var professoresIds = await _repository.GetProfessoresIdsDaTurmaAsync(id);

            return TurmaMapper.ToResponse(turma, qtdAlunos, qtdProfessores, alunosIds, professoresIds);
        }

        public async Task<TurmaResponseDTO> UpdateAsync(long id, TurmaRequestDTO dto)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"Turma com ID {id} não encontrada.");

            if (await _repository.ExisteTurmaAsync(dto.Nome, dto.Ano, dto.Turno, ignorarId: id))
                throw new InvalidOperationException("Já existe outra turma com esse nome, ano e turno.");

            var dadosNovos = TurmaMapper.ToModel(dto);
            var atualizada = await _repository.UpdateAsync(id, dadosNovos);
            return TurmaMapper.ToResponse(atualizada!);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"Turma com ID {id} não encontrada.");

            return await _repository.DeleteAsync(id);
        }

        // ── Aluno_Turma ───────────────────────────────────────────────────────

        public async Task MatricularAlunoAsync(long turmaId, long alunoId)
        {
            var turma = await _repository.GetByIdAsync(turmaId);
            if (turma == null)
                throw new KeyNotFoundException($"Turma com ID {turmaId} não encontrada.");

            if (await _repository.AlunoJaMatriculadoAsync(alunoId, turmaId))
                throw new InvalidOperationException("Aluno já matriculado nessa turma.");

            await _repository.MatricularAlunoAsync(alunoId, turmaId);
        }

        public async Task DesmatricularAlunoAsync(long turmaId, long alunoId)
        {
            var desmatriculado = await _repository.DesmatricularAlunoAsync(alunoId, turmaId);
            if (!desmatriculado)
                throw new KeyNotFoundException("Vínculo entre aluno e turma não encontrado.");
        }

        // ── Professor_Turma ───────────────────────────────────────────────────

        public async Task VincularProfessorAsync(long turmaId, long professorId)
        {
            var turma = await _repository.GetByIdAsync(turmaId);
            if (turma == null)
                throw new KeyNotFoundException($"Turma com ID {turmaId} não encontrada.");

            if (await _repository.ProfessorJaVinculadoAsync(professorId, turmaId))
                throw new InvalidOperationException("Professor já vinculado a essa turma.");

            await _repository.VincularProfessorAsync(professorId, turmaId);
        }

        public async Task DesvincularProfessorAsync(long turmaId, long professorId)
        {
            var desvinculado = await _repository.DesvincularProfessorAsync(professorId, turmaId);
            if (!desvinculado)
                throw new KeyNotFoundException("Vínculo entre professor e turma não encontrado.");
        }
    }
}

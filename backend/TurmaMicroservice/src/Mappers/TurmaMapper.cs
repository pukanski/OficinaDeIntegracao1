using TurmaAPI.DTOs;
using TurmaAPI.Model;

namespace TurmaAPI.Mappers
{
    public static class TurmaMapper
    {
        public static Turma ToModel(TurmaRequestDTO dto) => new Turma
        {
            Nome = dto.Nome.Trim(),
            Ano = dto.Ano.Trim(),
            Turno = dto.Turno.Trim(),
            Principal = dto.Principal
        };

        public static TurmaResponseDTO ToResponse(
    Turma turma,
    int qtdAlunos = 0,
    int qtdProfessores = 0,
    List<long>? alunosIds = null,
    List<long>? professoresIds = null)
        {
            return new TurmaResponseDTO
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Ano = turma.Ano,
                Turno = turma.Turno,
                Principal = turma.Principal,
                QtdAlunos = qtdAlunos,
                QtdProfessores = qtdProfessores,
                AlunosIds = alunosIds ?? new List<long>(),
                ProfessoresIds = professoresIds ?? new List<long>(),
                CriadoEm = turma.CriadoEm,
                AtualizadoEm = turma.AtualizadoEm
            };
        }

        public static List<TurmaResponseDTO> ToResponseList(
            List<(Turma turma, int qtdAlunos, int qtdProfessores)> turmas) =>
                turmas.Select(t => ToResponse(t.turma, t.qtdAlunos, t.qtdProfessores)).ToList();
    }
}

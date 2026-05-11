using TurmaAPI.src.DTOs;
using TurmaAPI.src.Model;

namespace TurmaAPI.src.Mappers
{
    public class TurmaMapper
    {

        public static Turma ToModel(TurmaRequestDTO requestDTO) => new Turma
        {
            nome = requestDTO.nome,
            ano = requestDTO.ano,
            turno = requestDTO.turno
        };

        public static TurmaResponseDTO ToResponse(Turma turmaModel, int qtdAlunos = 0) => new TurmaResponseDTO
        {
            id = turmaModel.id,
            nome = turmaModel.nome,
            ano = turmaModel.ano,
            turno = turmaModel.turno,
            qtdAlunos = qtdAlunos,
            criadoEm = turmaModel.criadoEm
        };

        public static List<TurmaResponseDTO> ToResponseList(List<(Turma turma, int qtd)> turmas) =>
            turmas.Select(t => ToResponse(t.turma, t.qtd)).ToList();
    }
}

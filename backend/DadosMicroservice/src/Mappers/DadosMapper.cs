using DadosAPI.DTOs;
using DadosAPI.Model;

namespace DadosAPI.Mappers
{
    public static class DadosMapper
    {
        public static RespostaAluno ToModel(RespostaAlunoRequestDTO dto) => new RespostaAluno
        {
            AlunoId       = dto.AlunoId,
            QuestaoId     = dto.QuestaoId,
            AlternativaId = dto.AlternativaId,
            Disciplina    = dto.Disciplina.Trim(),
            Vestibular    = dto.Vestibular.Trim().ToUpper(),
            AnoProva      = dto.AnoProva,
            Dificuldade   = dto.Dificuldade?.Trim(),
            Acertou       = dto.Acertou,
            RespondidoEm  = DateTime.UtcNow
        };

        public static RespostaAlunoResponseDTO ToResponse(RespostaAluno r) => new RespostaAlunoResponseDTO
        {
            Id            = r.Id,
            AlunoId       = r.AlunoId,
            QuestaoId     = r.QuestaoId,
            AlternativaId = r.AlternativaId,
            Disciplina    = r.Disciplina,
            Vestibular    = r.Vestibular,
            AnoProva      = r.AnoProva,
            Dificuldade   = r.Dificuldade,
            Acertou       = r.Acertou,
            RespondidoEm  = r.RespondidoEm
        };

        public static List<RespostaAlunoResponseDTO> ToResponseList(List<RespostaAluno> respostas) =>
            respostas.Select(ToResponse).ToList();
    }
}

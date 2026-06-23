using QuestaoAPI.DTOs;
using QuestaoAPI.Model;

namespace QuestaoAPI.Mappers
{
    public static class ProvaMapper
    {
        public static Prova ToModel(ProvaRequestDTO dto) => new Prova
        {
            Vestibular = dto.Vestibular.Trim().ToUpper(),
            Ano = dto.Ano,
            Edicao = dto.Edicao?.Trim()
        };

        public static ProvaResponseDTO ToResponse(Prova prova, int totalQuestoes = 0) => new ProvaResponseDTO
        {
            Id = prova.Id,
            Vestibular = prova.Vestibular,
            Ano = prova.Ano,
            Edicao = prova.Edicao,
            TotalQuestoes = totalQuestoes,
            CriadoEm = prova.CriadoEm,
            AtualizadoEm = prova.AtualizadoEm
        };

        public static List<ProvaResponseDTO> ToResponseList(List<(Prova prova, int total)> provas) =>
            provas.Select(p => ToResponse(p.prova, p.total)).ToList();
    }

    public static class QuestaoMapper
    {
        public static Questao ToModel(QuestaoRequestDTO dto) => new Questao
        {
            ProvaId = dto.ProvaId,
            Disciplina = dto.Disciplina.Trim(),
            Materia = dto.Materia?.Trim(),
            Enunciado = dto.Enunciado.Trim(),
            Dificuldade = dto.Dificuldade?.Trim(),
            Numero = dto.Numero
        };

        public static QuestaoResponseDTO ToResponse(Questao questao) => new QuestaoResponseDTO
        {
            Id = questao.Id,
            ProvaId = questao.ProvaId,
            ProvaDescricao = questao.Prova != null
                ? $"{questao.Prova.Vestibular} {questao.Prova.Ano}"
                : string.Empty,
            Disciplina = questao.Disciplina,
            Materia = questao.Materia,
            Enunciado = questao.Enunciado,
            Dificuldade = questao.Dificuldade,
            Numero = questao.Numero,
            Alternativas = questao.Alternativas
                .OrderBy(a => a.Letra)
                .Select(AlternativaMapper.ToResponse)
                .ToList(),
            CriadoEm = questao.CriadoEm,
            AtualizadoEm = questao.AtualizadoEm
        };

        public static List<QuestaoResponseDTO> ToResponseList(List<Questao> questoes) =>
            questoes.Select(ToResponse).ToList();
    }

    public static class AlternativaMapper
    {
        public static Alternativa ToModel(AlternativaRequestDTO dto) => new Alternativa
        {
            QuestaoId = dto.QuestaoId,
            Letra = dto.Letra.Trim().ToUpper(),
            Texto = dto.Texto.Trim(),
            Correta = dto.Correta
        };

        public static AlternativaResponseDTO ToResponse(Alternativa alternativa) => new AlternativaResponseDTO
        {
            Id = alternativa.Id,
            QuestaoId = alternativa.QuestaoId,
            Letra = alternativa.Letra,
            Texto = alternativa.Texto,
            Correta = alternativa.Correta
        };

        public static List<AlternativaResponseDTO> ToResponseList(List<Alternativa> alternativas) =>
            alternativas.Select(ToResponse).ToList();
    }
}

using System.ComponentModel.DataAnnotations;

namespace DadosAPI.DTOs
{
    // ═══════════════════════════════════════════════════════════
    // REGISTRAR RESPOSTA
    // ═══════════════════════════════════════════════════════════

    /// <summary>Dados para registrar a resposta de um aluno a uma questão.</summary>
    public class RespostaAlunoRequestDTO
    {
        /// <example>1</example>
        [Required(ErrorMessage = "O ID do aluno é obrigatório.")]
        public long AlunoId { get; set; }

        /// <example>10</example>
        [Required(ErrorMessage = "O ID da questão é obrigatório.")]
        public long QuestaoId { get; set; }

        /// <example>3</example>
        [Required(ErrorMessage = "O ID da alternativa é obrigatório.")]
        public long AlternativaId { get; set; }

        /// <example>Matemática</example>
        [Required(ErrorMessage = "A disciplina é obrigatória.")]
        [MaxLength(80)]
        public string Disciplina { get; set; }

        /// <example>ENEM</example>
        [Required(ErrorMessage = "O vestibular é obrigatório.")]
        [MaxLength(80)]
        public string Vestibular { get; set; }

        /// <example>2024</example>
        [Required(ErrorMessage = "O ano da prova é obrigatório.")]
        public int AnoProva { get; set; }

        /// <example>Médio</example>
        public string? Dificuldade { get; set; }

        /// <example>true</example>
        [Required(ErrorMessage = "O resultado é obrigatório.")]
        public bool Acertou { get; set; }
    }

    /// <summary>Dados retornados após registrar uma resposta.</summary>
    public class RespostaAlunoResponseDTO
    {
        /// <example>1</example>
        public long Id { get; set; }

        /// <example>1</example>
        public long AlunoId { get; set; }

        /// <example>10</example>
        public long QuestaoId { get; set; }

        /// <example>3</example>
        public long AlternativaId { get; set; }

        /// <example>Matemática</example>
        public string Disciplina { get; set; }

        /// <example>ENEM</example>
        public string Vestibular { get; set; }

        /// <example>2024</example>
        public int AnoProva { get; set; }

        /// <example>Médio</example>
        public string? Dificuldade { get; set; }

        /// <example>true</example>
        public bool Acertou { get; set; }

        public DateTime RespondidoEm { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // DESEMPENHO GERAL
    // ═══════════════════════════════════════════════════════════

    /// <summary>Desempenho geral do aluno na plataforma.</summary>
    public class DesempenhoGeralDTO
    {
        /// <example>1</example>
        public long AlunoId { get; set; }

        /// <example>120</example>
        public int TotalRespondidas { get; set; }

        /// <example>84</example>
        public int TotalAcertos { get; set; }

        /// <example>36</example>
        public int TotalErros { get; set; }

        /// <example>70.0</example>
        public double PercentualAcerto { get; set; }

        /// <example>2026-01-01T00:00:00Z</example>
        public DateTime? PrimeiraResposta { get; set; }

        /// <example>2026-06-01T00:00:00Z</example>
        public DateTime? UltimaResposta { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // DESEMPENHO POR DISCIPLINA
    // ═══════════════════════════════════════════════════════════

    /// <summary>Desempenho do aluno agrupado por disciplina.</summary>
    public class DesempenhoPorDisciplinaDTO
    {
        /// <example>1</example>
        public long AlunoId { get; set; }

        public List<DisciplinaDesempenhoDTO> Disciplinas { get; set; } = new();
    }

    public class DisciplinaDesempenhoDTO
    {
        /// <example>Matemática</example>
        public string Disciplina { get; set; }

        /// <example>40</example>
        public int TotalRespondidas { get; set; }

        /// <example>30</example>
        public int TotalAcertos { get; set; }

        /// <example>10</example>
        public int TotalErros { get; set; }

        /// <example>75.0</example>
        public double PercentualAcerto { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // DESEMPENHO POR VESTIBULAR
    // ═══════════════════════════════════════════════════════════

    /// <summary>Desempenho do aluno agrupado por vestibular.</summary>
    public class DesempenhoPorVestibularDTO
    {
        /// <example>1</example>
        public long AlunoId { get; set; }

        public List<VestibularDesempenhoDTO> Vestibulares { get; set; } = new();
    }

    public class VestibularDesempenhoDTO
    {
        /// <example>ENEM</example>
        public string Vestibular { get; set; }

        /// <example>2024</example>
        public int AnoProva { get; set; }

        /// <example>45</example>
        public int TotalRespondidas { get; set; }

        /// <example>32</example>
        public int TotalAcertos { get; set; }

        /// <example>13</example>
        public int TotalErros { get; set; }

        /// <example>71.1</example>
        public double PercentualAcerto { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // HISTÓRICO
    // ═══════════════════════════════════════════════════════════

    /// <summary>Histórico paginado de respostas do aluno.</summary>
    public class HistoricoRespostasDTO
    {
        /// <example>1</example>
        public long AlunoId { get; set; }

        /// <example>120</example>
        public int TotalRegistros { get; set; }

        /// <example>1</example>
        public int PaginaAtual { get; set; }

        /// <example>10</example>
        public int TamanhoPagina { get; set; }

        /// <example>12</example>
        public int TotalPaginas { get; set; }

        public List<RespostaAlunoResponseDTO> Respostas { get; set; } = new();
    }
}

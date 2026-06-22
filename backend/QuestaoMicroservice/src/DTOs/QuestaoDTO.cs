using System.ComponentModel.DataAnnotations;

namespace QuestaoAPI.DTOs
{
    // ═══════════════════════════════════════════════════════════
    // PROVA
    // ═══════════════════════════════════════════════════════════

    /// <summary>Dados para cadastrar ou atualizar uma prova.</summary>
    public class ProvaRequestDTO
    {
        /// <example>ENEM</example>
        [Required(ErrorMessage = "O vestibular é obrigatório.")]
        [MaxLength(80)]
        public string Vestibular { get; set; }

        /// <example>2024</example>
        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(1900, 2100, ErrorMessage = "Ano inválido.")]
        public int Ano { get; set; }

        /// <example>2º Dia</example>
        public string? Edicao { get; set; }
    }

    /// <summary>Dados retornados de uma prova.</summary>
    public class ProvaResponseDTO
    {
        /// <example>1</example>
        public long Id { get; set; }

        /// <example>ENEM</example>
        public string Vestibular { get; set; }

        /// <example>2024</example>
        public int Ano { get; set; }

        /// <example>2º Dia</example>
        public string? Edicao { get; set; }

        /// <example>45</example>
        public int TotalQuestoes { get; set; }

        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // QUESTAO
    // ═══════════════════════════════════════════════════════════

    /// <summary>Dados para cadastrar ou atualizar uma questão.</summary>
    public class QuestaoRequestDTO
    {
        /// <example>1</example>
        [Required(ErrorMessage = "O ID da prova é obrigatório.")]
        public long ProvaId { get; set; }

        /// <example>Matemática</example>
        [Required(ErrorMessage = "A disciplina é obrigatória.")]
        [MaxLength(80)]
        public string Disciplina { get; set; }

        /// <example>Geometria Plana</example>
        public string? Materia { get; set; }

        /// <example>Se um triângulo tem lados 3, 4 e 5, qual é a área?</example>
        [Required(ErrorMessage = "O enunciado é obrigatório.")]
        public string Enunciado { get; set; }

        /// <example>Médio</example>
        public string? Dificuldade { get; set; }

        /// <example>1</example>
        [Required(ErrorMessage = "O número da questão é obrigatório.")]
        public int Numero { get; set; }
    }

    /// <summary>Dados retornados de uma questão.</summary>
    public class QuestaoResponseDTO
    {
        /// <example>1</example>
        public long Id { get; set; }

        /// <example>1</example>
        public long ProvaId { get; set; }

        /// <example>ENEM 2024</example>
        public string ProvaDescricao { get; set; }

        /// <example>Matemática</example>
        public string Disciplina { get; set; }

        /// <example>Geometria Plana</example>
        public string? Materia { get; set; }

        /// <example>Se um triângulo tem lados 3, 4 e 5, qual é a área?</example>
        public string Enunciado { get; set; }

        /// <example>Médio</example>
        public string? Dificuldade { get; set; }

        /// <example>1</example>
        public int Numero { get; set; }

        public List<AlternativaResponseDTO> Alternativas { get; set; } = new();

        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // ALTERNATIVA
    // ═══════════════════════════════════════════════════════════

    /// <summary>Dados para cadastrar ou atualizar uma alternativa.</summary>
    public class AlternativaRequestDTO
    {
        /// <example>1</example>
        [Required(ErrorMessage = "O ID da questão é obrigatório.")]
        public long QuestaoId { get; set; }

        /// <example>A</example>
        [Required(ErrorMessage = "A letra é obrigatória.")]
        [MaxLength(1)]
        [RegularExpression("^[A-E]$", ErrorMessage = "A letra deve ser A, B, C, D ou E.")]
        public string Letra { get; set; }

        /// <example>6 unidades quadradas</example>
        [Required(ErrorMessage = "O texto é obrigatório.")]
        [MaxLength(500)]
        public string Texto { get; set; }

        /// <example>true</example>
        public bool Correta { get; set; } = false;
    }

    /// <summary>Dados retornados de uma alternativa.</summary>
    public class AlternativaResponseDTO
    {
        /// <example>1</example>
        public long Id { get; set; }

        /// <example>1</example>
        public long QuestaoId { get; set; }

        /// <example>A</example>
        public string Letra { get; set; }

        /// <example>6 unidades quadradas</example>
        public string Texto { get; set; }

        /// <example>true</example>
        public bool Correta { get; set; }
    }
}

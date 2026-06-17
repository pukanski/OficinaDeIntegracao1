using System.ComponentModel.DataAnnotations;

namespace TurmaAPI.DTOs
{
    /// <summary>Dados necessários para cadastrar ou atualizar uma turma.</summary>
    public class TurmaRequestDTO
    {
        /// <example>Turma A</example>
        [Required(ErrorMessage = "O nome da turma é obrigatório.")]
        [MaxLength(80)]
        public string Nome { get; set; }

        /// <example>2026</example>
        [Required(ErrorMessage = "O ano é obrigatório.")]
        [MaxLength(4)]
        public string Ano { get; set; }

        /// <example>Matutino</example>
        [Required(ErrorMessage = "O turno é obrigatório.")]
        [MaxLength(20)]
        public string Turno { get; set; }
    }

    /// <summary>Dados retornados de uma turma.</summary>
    public class TurmaResponseDTO
    {
        /// <example>1</example>
        public long Id { get; set; }

        /// <example>Turma A</example>
        public string Nome { get; set; }

        /// <example>2026</example>
        public string Ano { get; set; }

        /// <example>Matutino</example>
        public string Turno { get; set; }

        /// <example>10</example>
        public int QtdAlunos { get; set; }

        /// <example>3</example>
        public int QtdProfessores { get; set; }

        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }

    /// <summary>Dados para matricular um aluno em uma turma.</summary>
    public class MatricularAlunoDTO
    {
        /// <example>1</example>
        [Required(ErrorMessage = "O ID do aluno é obrigatório.")]
        public long AlunoId { get; set; }
    }
}

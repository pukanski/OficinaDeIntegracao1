using System.ComponentModel.DataAnnotations;

namespace TurmaAPI.DTOs
{
    public class TurmaRequestDTO
    {
        [Required(ErrorMessage = "O nome da turma é obrigatório.")]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório.")]
        [MaxLength(4)]
        public string Ano { get; set; }

        [Required(ErrorMessage = "O turno é obrigatório.")]
        [MaxLength(20)]
        public string Turno { get; set; }
    }

    public class TurmaResponseDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Ano { get; set; }
        public string Turno { get; set; }
        public int QtdAlunos { get; set; }
        public int QtdProfessores { get; set; }
        public List<long> AlunosIds { get; set; } = new();
        public List<long> ProfessoresIds { get; set; } = new();
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }

    public class MatricularAlunoDTO
    {
        [Required(ErrorMessage = "O ID do aluno é obrigatório.")]
        public long AlunoId { get; set; }
    }
}
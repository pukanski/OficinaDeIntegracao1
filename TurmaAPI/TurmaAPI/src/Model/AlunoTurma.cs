using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurmaAPI.Model
{
    [Table("Aluno_Turma")]
    public class AlunoTurma
    {
        [Required]
        public long AlunoId { get; set; }

        [Required]
        public long TurmaId { get; set; }

        public DateTime MatriculadoEm { get; set; } = DateTime.UtcNow;

        public Turma Turma { get; set; }
    }
}

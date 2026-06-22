using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurmaAPI.Model
{
    [Table("Aluno_Turma")]
    public class AlunoTurma
    {
        [Required]
        [Column("AlunoId")]
        public long AlunoId { get; set; }

        [Required]
        [Column("TurmaId")]
        public long TurmaId { get; set; }

        [Column("MatriculadoEm")]
        public DateTime MatriculadoEm { get; set; } = DateTime.UtcNow;

        [Column("Turma")]
        public Turma Turma { get; set; }
    }
}

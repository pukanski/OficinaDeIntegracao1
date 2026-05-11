using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurmaAPI.src.Model
{
    [Table("aluno_turma")]
    public class AlunoTurma
    {

        [Required]
        [Column("aluno_id")]
        public long aluno_id;

        [Required]
        [Column("turma_id")]
        public long turma_id { get; set; }

        [Required]
        [Column("matriculado_em")]
        public DateTime matriculadoEm { get; set; } = DateTime.UtcNow;

        public Turma turma { get; set; }
    }
}

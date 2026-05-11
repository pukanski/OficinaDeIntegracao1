using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurmaAPI.src.Model
{
    [Table("professor_turma")]
    public class ProfessorTurma
    {
        [Required]
        [Column("professor_id")]
        public long professor_id { get; set; }

        [Required]
        [Column("turma_id")]
        public long turma_id { get; set; }

        [Required]
        [Column("vinculado_em")]
        public DateTime vinculadoEm { get; set; } = DateTime.UtcNow;
    }
}

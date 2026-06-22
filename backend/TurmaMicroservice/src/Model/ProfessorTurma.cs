using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurmaAPI.Model
{
    [Table("Professor_Turma")]
    public class ProfessorTurma
    {
        [Required]
        [Column("ProfessorId")]
        public long ProfessorId { get; set; }

        [Required]
        [Column("TurmaId")]
        public long TurmaId { get; set; }

        [Column("VinculadoEm")]
        public DateTime VinculadoEm { get; set; } = DateTime.UtcNow;

        [Column("Turma")]
        public Turma Turma { get; set; }
    }
}

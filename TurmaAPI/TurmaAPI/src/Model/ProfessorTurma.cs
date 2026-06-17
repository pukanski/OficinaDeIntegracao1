using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurmaAPI.Model
{
    [Table("Professor_Turma")]
    public class ProfessorTurma
    {
        [Required]
        public long ProfessorId { get; set; }

        [Required]
        public long TurmaId { get; set; }

        public DateTime VinculadoEm { get; set; } = DateTime.UtcNow;

        public Turma Turma { get; set; }
    }
}

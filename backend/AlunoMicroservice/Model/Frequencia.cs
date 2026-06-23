using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlunoAPI.Model
{
    [Table("Frequencia")]
    public class Frequencia
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("TurmaId")]
        public long TurmaId { get; set; }

        [Required]
        [Column("AlunoId")]
        public long AlunoId { get; set; }

        [Required]
        [Column("Data")]
        public DateOnly Data { get; set; }

        [Column("Presente")]
        public bool Presente { get; set; }

        [Column("CriadoEm")]
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}

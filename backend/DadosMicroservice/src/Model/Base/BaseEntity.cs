using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DadosAPI.Model.Base
{
    public class BaseEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("CriadoEm")]
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        [Column("AtualizadoEm")]
        public DateTime? AtualizadoEm { get; set; }
    }
}

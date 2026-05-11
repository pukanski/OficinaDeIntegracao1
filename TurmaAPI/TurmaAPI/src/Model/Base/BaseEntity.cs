using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurmaAPI.src.Model.Base
{
    public class BaseEntity
    {

        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public DateTime criadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? atualizadoEm { get; set; } 
    }
}

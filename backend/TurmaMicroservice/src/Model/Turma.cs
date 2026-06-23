using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurmaAPI.Model.Base;

namespace TurmaAPI.Model
{
    [Table("Turmas")]
    public class Turma : BaseEntity
    {
        [Required(ErrorMessage = "O nome da turma é obrigatório.")]
        [MaxLength(80)]
        [Column("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório.")]
        [MaxLength(4)]
        [Column("Ano")]
        public string Ano { get; set; }

        [Required(ErrorMessage = "O turno é obrigatório.")]
        [MaxLength(20)]
        [Column("Turno")]
        public string Turno { get; set; }

        [Column("Principal")]
        public bool Principal { get; set; } = false;
    }
}

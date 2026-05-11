using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurmaAPI.src.Model.Base;

namespace TurmaAPI.src.Model
{
    [Table("turmas")]
    public class Turma : BaseEntity
    {

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Column("nome")]
        [MaxLength(80)]
        public string nome { get; set; }

        [Required (ErrorMessage = "O ano é obrigatório")]
        [Column("ano")]
        [MaxLength(4)]
        public string ano { get; set; }

        [Required(ErrorMessage = "O turno é obrigatório")]
        [Column("turno")]
        [MaxLength(80)]
        public string turno { get; set; }

        [Required(ErrorMessage = "A quantidade de alunos é obrigatória")]
        [Column("qtd_alunos")]
        public int qtdAlunos { get; set; }
    }
}

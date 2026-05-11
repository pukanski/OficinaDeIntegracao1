using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurmaAPI.src.DTOs
{
    /// <summary>Dados necessários para cadastrar ou atualizar uma turma.</summary>
    public class TurmaRequestDTO
    {
        [Required]
        [Column("nome")]
        [MaxLength(80)]
        public string nome { get; set; }

        [Required]
        [Column("ano")]
        [MaxLength(4)]
        public string ano { get; set; }

        [Required]
        [Column("turno")]
        [MaxLength(80)]
        public string turno { get; set; }
    }
}

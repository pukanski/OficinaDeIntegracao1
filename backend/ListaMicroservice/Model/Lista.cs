using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ListaAPI.Model.Base;

namespace ListaAPI.Model
{
    [Table("Listas")]
    public class Lista : BaseEntity
    {
        [Required]
        [MaxLength(120)]
        [Column("Titulo")]
        public string Titulo { get; set; }

        [Required]
        [Column("ProfessorId")]
        public long ProfessorId { get; set; }

        [Column("DataVencimento")]
        public DateTime? DataVencimento { get; set; }
    }
}
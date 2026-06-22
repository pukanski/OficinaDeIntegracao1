using System.ComponentModel.DataAnnotations.Schema;

namespace ListaAPI.Model
{
    [Table("Lista_Turma")]
    public class ListaTurma
    {
        [Column("ListaId")]
        public long ListaId { get; set; }

        [Column("TurmaId")]
        public long TurmaId { get; set; }

        [Column("AtribuidoEm")]
        public DateTime AtribuidoEm { get; set; } = DateTime.UtcNow;
    }
}
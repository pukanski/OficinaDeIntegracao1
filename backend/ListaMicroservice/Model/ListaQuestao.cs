using System.ComponentModel.DataAnnotations.Schema;

namespace ListaAPI.Model
{
    [Table("Lista_Questao")]
    public class ListaQuestao
    {
        [Column("ListaId")]
        public long ListaId { get; set; }

        [Column("QuestaoId")]
        public long QuestaoId { get; set; }

        [Column("AdicionadoEm")]
        public DateTime AdicionadoEm { get; set; } = DateTime.UtcNow;
    }
}
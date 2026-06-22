using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuestaoAPI.Model.Base;

namespace QuestaoAPI.Model
{
    [Table("Alternativas")]
    public class Alternativa : BaseEntity
    {
        [Required(ErrorMessage = "O ID da questão é obrigatório.")]
        [Column("QuestaoId")] public long QuestaoId { get; set; }

        [Required(ErrorMessage = "A letra é obrigatória.")]
        [MaxLength(1)]
        [Column("Letra")] public string Letra { get; set; }

        [Required(ErrorMessage = "O texto é obrigatório.")]
        [MaxLength(500)]
        [Column("Texto")] public string Texto { get; set; }

        [Column("Correta")] public bool Correta { get; set; } = false;

        // Navegação
        public Questao Questao { get; set; }
    }
}

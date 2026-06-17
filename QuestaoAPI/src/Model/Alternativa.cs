using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuestaoAPI.Model.Base;

namespace QuestaoAPI.Model
{
    [Table("Alternativas")]
    public class Alternativa : BaseEntity
    {
        [Required(ErrorMessage = "O ID da questão é obrigatório.")]
        public long QuestaoId { get; set; }

        [Required(ErrorMessage = "A letra é obrigatória.")]
        [MaxLength(1)]
        public string Letra { get; set; }

        [Required(ErrorMessage = "O texto é obrigatório.")]
        [MaxLength(500)]
        public string Texto { get; set; }

        public bool Correta { get; set; } = false;

        // Navegação
        public Questao Questao { get; set; }
    }
}

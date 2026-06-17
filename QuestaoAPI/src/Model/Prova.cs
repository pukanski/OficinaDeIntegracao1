using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuestaoAPI.Model.Base;

namespace QuestaoAPI.Model
{
    [Table("Provas")]
    public class Prova : BaseEntity
    {
        [Required(ErrorMessage = "O nome do vestibular é obrigatório.")]
        [MaxLength(80)]
        public string Vestibular { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório.")]
        public int Ano { get; set; }

        [MaxLength(40)]
        public string? Edicao { get; set; }

        // Navegação
        public ICollection<Questao> Questoes { get; set; } = new List<Questao>();
    }
}

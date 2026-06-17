using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuestaoAPI.Model.Base;

namespace QuestaoAPI.Model
{
    [Table("Questoes")]
    public class Questao : BaseEntity
    {
        [Required(ErrorMessage = "O ID da prova é obrigatório.")]
        public long ProvaId { get; set; }

        [Required(ErrorMessage = "A disciplina é obrigatória.")]
        [MaxLength(80)]
        public string Disciplina { get; set; }

        [MaxLength(80)]
        public string? Materia { get; set; }

        [Required(ErrorMessage = "O enunciado é obrigatório.")]
        public string Enunciado { get; set; }

        [MaxLength(20)]
        public string? Dificuldade { get; set; }

        public int Numero { get; set; }

        // Navegação
        public Prova Prova { get; set; }
        public ICollection<Alternativa> Alternativas { get; set; } = new List<Alternativa>();
    }
}

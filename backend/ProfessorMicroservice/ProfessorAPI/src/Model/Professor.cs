using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfessorAPI.src.Model
{
    [Table ("Professores")]
    public class Professor: Base.BaseEntity
    {

        [Required(ErrorMessage = "Siape obrigatório!")]
        [MaxLength(7)]
        public string Siape { get; set; }
        
        [Required (ErrorMessage = "O primeiro nome deve ser informado obrigatoriamente.")]
        [MaxLength(80)]
        public string PrimeiroNome { get; set; }

        [Required(ErrorMessage = "O último nome deve ser informado obrigatoriamente.")]
        [MaxLength(80)]
        public string UltimoNome { get; set; }

        [Required(ErrorMessage = "O email deve ser informado obrigatoriamente.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A disciplina deve ser informada obrigatoriamente.")]
        [MaxLength(80)]
        public string Disciplina { get; set; }

    }
}

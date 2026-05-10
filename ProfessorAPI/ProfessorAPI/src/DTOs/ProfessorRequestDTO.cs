using System.ComponentModel.DataAnnotations;

namespace ProfessorAPI.src.DTOs
{
    public class ProfessorRequestDTO
    {


        [Required(ErrorMessage = "Siape obrigatório!")]
        [MaxLength(7)]
        public string siape { get; set; }

        [Required(ErrorMessage = "O primeiro nome deve ser informado obrigatoriamente.")]
        [MaxLength(80)]
        public string primeiroNome { get; set; }
        [Required(ErrorMessage = "O último nome deve ser informado obrigatoriamente.")]
        [MaxLength(80)]
        public string ultimoNome { get; set; }

        [Required(ErrorMessage = "O email deve ser informado obrigatoriamente.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string email { get; set; }

        [Required(ErrorMessage = "A disciplina deve ser informada obrigatoriamente.")]
        [MaxLength(80)]
        public string disciplina { get; set; }
    }
}

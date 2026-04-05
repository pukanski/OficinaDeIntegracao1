using System.ComponentModel.DataAnnotations;

namespace AlunoAPI.DTOs
{
    public class AlunoRequestDTO
    {
        [Required(ErrorMessage = "O primeiro nome deve ser informado obrigatoriamente.")]
        [MaxLength(80)]
        public string PrimeiroNome { get; set; }

        [Required(ErrorMessage = "O último nome deve ser informado obrigatoriamente.")]
        [MaxLength(80)]
        public string UltimoNome { get; set; }

        [Required(ErrorMessage = "O email deve ser informado obrigatoriamente.")]
        [EmailAddress(ErrorMessage = "Email informado inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O RA deve ser informado obrigatoriamente")]
        public string RA { get; set; }
    }
}

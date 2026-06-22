using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlunoAPI.Model
{
    [Table("Aluno")]
    public class Aluno : Base.BaseEntity
    {
        [Required]
        [Column("AuthId")]
        public Guid AuthId { get; set; }
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

using System.ComponentModel.DataAnnotations;

namespace TurmaAPI.src.DTOs
{
    public class MatricularAlunoDTO
    {
        [Required(ErrorMessage = "O campo IdAluno é obrigatório")]
        public long alunoId { get; set; }
    }
}

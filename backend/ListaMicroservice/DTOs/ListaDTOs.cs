using System.ComponentModel.DataAnnotations;

namespace ListaAPI.DTOs
{
    public class ListaRequestDTO
    {
        [Required]
        [MaxLength(120)]
        public string Titulo { get; set; }

        [Required]
        public long ProfessorId { get; set; }

        public DateTime? DataVencimento { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "A lista precisa de pelo menos uma questão.")]
        public List<long> QuestoesIds { get; set; }

        public List<long> TurmasIds { get; set; } = new();
    }

    public class ListaResponseDTO
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public long ProfessorId { get; set; }
        public DateTime? DataVencimento { get; set; }
        public List<long> QuestoesIds { get; set; } = new();
        public List<long> TurmasIds { get; set; } = new();
        public DateTime CriadoEm { get; set; }
    }
}
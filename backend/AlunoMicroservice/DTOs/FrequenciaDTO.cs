using System.ComponentModel.DataAnnotations;

namespace AlunoAPI.DTOs
{
    public class RegistroFrequenciaDTO
    {
        public long AlunoId { get; set; }
        public bool Presente { get; set; }
    }

    public class SalvarChamadaRequestDTO
    {
        [Required]
        public long TurmaId { get; set; }

        [Required]
        public DateOnly Data { get; set; }

        [Required]
        public List<RegistroFrequenciaDTO> Registros { get; set; } = new();
    }

    public class FrequenciaAlunoResponseDTO
    {
        public long AlunoId { get; set; }
        public bool Presente { get; set; }
    }

    public class ChamadaResponseDTO
    {
        public long TurmaId { get; set; }
        public DateOnly Data { get; set; }
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
        public List<FrequenciaAlunoResponseDTO> Registros { get; set; } = new();
    }

    public class HistoricoItemDTO
    {
        public long TurmaId { get; set; }
        public DateOnly Data { get; set; }
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
    }

    public class PercentualPresencaDTO
    {
        public long AlunoId { get; set; }
        public int TotalAulas { get; set; }
        public int Presencas { get; set; }
        public double Percentual { get; set; }
    }
}

namespace ProfessorAPI.src.DTOs
{
    public class ProfessorResponseDTO
    {

        public long Id { get; set; }
        public string Siape { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string Email { get; set; }
        public string Disciplina { get; set; }
        public DateTime criadoEm { get; set; }
    }
}

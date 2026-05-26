namespace ProfessorAPI.src.DTOs
{
    public class ProfessorResponseDTO
    {

        public long id { get; set; }
        public string siape { get; set; }
        public string primeiroNome { get; set; }
        public string ultimoNome { get; set; }
        public string email { get; set; }
        public string disciplina { get; set; }
        public DateTime criadoEm { get; set; }
    }
}

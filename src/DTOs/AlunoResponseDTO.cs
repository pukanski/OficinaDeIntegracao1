namespace AlunoAPI.DTOs
{
    public class AlunoResponseDTO
    {
    
        public long Id { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string Email { get; set; }
        public string RA { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}

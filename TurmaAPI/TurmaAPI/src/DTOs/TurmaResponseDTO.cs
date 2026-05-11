namespace TurmaAPI.src.DTOs
{
    public class TurmaResponseDTO
    {

        public long id { get; set; }
        public string nome { get; set; }
        public string ano { get; set; }
        public string turno { get; set; }
        public int qtdAlunos { get; set; }

        public DateTime criadoEm { get; set; }

    }
}

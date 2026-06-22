public class AdminUsuarioRequestDTO
{
    public string Email { get; set; }
    public string Senha { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
    public string Tipo { get; set; } // 'Aluno' ou 'Professor'
    public string? Ra { get; set; }
    public string? Siape { get; set; }
    public string? Disciplina { get; set; }
}
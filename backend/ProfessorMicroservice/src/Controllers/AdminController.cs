using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ProfessorAPI.src.Model;
using ProfessorAPI.src.Data;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppDbContext _context;
    private readonly string _supabaseUrl;
    private readonly string _serviceKey;

    public AdminController(IHttpClientFactory httpClientFactory, AppDbContext context, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
        _supabaseUrl = config["Supabase:Url"] ?? "";
        _serviceKey = config["Supabase:ServiceKey"] ?? "";
    }

    [HttpPost("criar-usuario")]
    public async Task<IActionResult> CriarUsuario([FromBody] AdminUsuarioRequestDTO dto)
    {
        try
        {
            // Cria usuário no Supabase Auth via REST API
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _serviceKey);
            client.DefaultRequestHeaders.Add("apikey", _serviceKey);

            var body = JsonSerializer.Serialize(new
            {
                email = dto.Email,
                password = dto.Senha,
                email_confirm = true
            });

            var response = await client.PostAsync(
                $"{_supabaseUrl}/auth/v1/admin/users",
                new StringContent(body, Encoding.UTF8, "application/json")
            );

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest(new { error = $"Supabase Auth error: {responseBody}" });

            var json = JsonDocument.Parse(responseBody);
            var authId = Guid.Parse(json.RootElement.GetProperty("id").GetString()!);

            // Insere no banco
            if (dto.Tipo == "Aluno")
            {
                var aluno = new AlunoEntity
                {
                    AuthId = authId,
                    PrimeiroNome = dto.PrimeiroNome,
                    UltimoNome = dto.UltimoNome,
                    Email = dto.Email,
                    RA = dto.Ra ?? ""
                };
                _context.Alunos.Add(aluno);
            }
            else
            {
                var prof = new Professor
                {
                    AuthId = authId,
                    PrimeiroNome = dto.PrimeiroNome,
                    UltimoNome = dto.UltimoNome,
                    Email = dto.Email,
                    Siape = dto.Siape ?? "",
                    Disciplina = dto.Disciplina ?? ""
                };
                _context.Professores.Add(prof);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário criado com sucesso", authId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("usuarios")]
    public async Task<IActionResult> ListarUsuarios()
    {
        var professores = await _context.Professores
            .Select(p => new
            {
                id = p.Id,
                nome = p.PrimeiroNome + " " + p.UltimoNome,
                email = p.Email,
                tipo = "Professor",
                status = "Ativo"
            }).ToListAsync();

        var alunos = await _context.Alunos
            .Select(a => new
            {
                id = a.Id,
                nome = a.PrimeiroNome + " " + a.UltimoNome,
                email = a.Email,
                tipo = "Aluno",
                status = "Ativo"
            }).ToListAsync();

        return Ok(new { professores, alunos });
    }
}

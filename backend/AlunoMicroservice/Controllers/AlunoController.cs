using AlunoAPI.DTOs;
using AlunoAPI.Model;
using AlunoAPI.Services;
using AlunoAPI.Data; // Correção
using AlunoAPI.Mappers; // Correção
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; // Correção

namespace AlunoAPI.Controllers
{
    /// <summary>
    /// Gerenciamento de alunos da plataforma.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoService _service;
        private readonly AppDbContext _context;
        public AlunoController(IAlunoService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }


        /// <summary>Cadastra um novo aluno.</summary>
        /// <param name="dto">Dados do aluno a ser cadastrado.</param>
        /// <response code="201">Aluno cadastrado com sucesso.</response>
        [HttpPost("criar-aluno")]
        public async Task<IActionResult> Create([FromBody] AlunoRequestDTO dto)
        {
            var alunoCriado = await _service.CreateAlunoAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { }, alunoCriado);
        }

        [HttpGet("alunos")]
        public async Task<IActionResult> GetAll()
        {
            var all = await _service.GetAllAlunosAsync();
            return Ok(all);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var aluno = await _service.GetAlunoByIdAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return Ok(aluno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] AlunoRequestDTO dto)
        {
            var alunoAtualizado = await _service.UpdateAlunoByIdAsync(id, dto);
            if (alunoAtualizado == null)
            {
                return NotFound();
            }
            return Ok(alunoAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var alunoDeletado = await _service.DeleteAlunoByIdAsync(id);
            if (alunoDeletado == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<AlunoResponseDTO>> GetMe()
        {
            var authIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(authIdClaim)) return Unauthorized("Token inválido ou sem sub claim.");

            var authId = Guid.Parse(authIdClaim);
            var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.AuthId == authId);

            if (aluno == null) return NotFound("Perfil de aluno não encontrado no banco de dados.");

            return Ok(AlunoMapper.ToResponse(aluno));
        }

    }
}

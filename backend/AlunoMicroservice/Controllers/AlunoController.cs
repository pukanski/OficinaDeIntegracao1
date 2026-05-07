using AlunoAPI.DTOs;
using AlunoAPI.Model;
using AlunoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
        public AlunoController(IAlunoService service) => _service = service;


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
            if(alunoAtualizado == null)
            {
                return NotFound();
            }
            return Ok(alunoAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var alunoDeletado = await _service.DeleteAlunoByIdAsync(id);
            if(alunoDeletado == null)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using ProfessorAPI.src.DTOs;
using ProfessorAPI.src.Model;
using ProfessorAPI.src.Services;

namespace ProfessorAPI.src.Controllers
{
    /// <summary>Gerenciamento de professores da plataforma.</summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _service;

        public ProfessorController(IProfessorService service)
        {
            _service = service;
        }


        [HttpPost("criar_professor")]
        public async Task<IActionResult> CreateProfessor([FromBody] ProfessorRequestDTO professorRequest)
        {

            try
            {
                var professorCriado = await _service.CreateProfessorAsync(professorRequest);
                return CreatedAtAction(nameof(GetById), new { id = professorCriado.id }, professorCriado);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("professores")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var professores = await _service.GetAllProfessoresAsync();
                return Ok(professores);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("professor/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var professor = await _service.GetProfessorByIdAsync(id);
                if (professor == null)
                {
                    return NotFound();
                }
                return Ok(professor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Atualiza os dados de um professor.</summary>
        /// <response code="200">Professor atualizado com sucesso.</response>
        /// <response code="404">Professor não encontrado.</response>
        /// <response code="400">SIAPE ou Email já em uso.</response>
        [HttpPut ("atualizar_professor/{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, [FromBody] ProfessorRequestDTO professorRequest)
        {
            try
            {
                var professorAtualizado = await _service.UpdateProfessorAsync(id, professorRequest);
                if (professorAtualizado == null)
                {
                    return NotFound();
                }
                return Ok(professorAtualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove um professor pelo ID.</summary>
        /// <response code="204">Professor removido com sucesso.</response>
        /// <response code="404">Professor não encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deletado = await _service.DeleteProfessorAsync(id);
            if (!deletado) return NotFound(new { mensagem = "Professor não encontrado." });
            return NoContent();
        }
    }
}

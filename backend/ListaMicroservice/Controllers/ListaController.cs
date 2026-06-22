using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ListaAPI.DTOs;
using ListaAPI.Services;

namespace ListaAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Lista")]
    public class ListaController : ControllerBase
    {
        private readonly IListaService _service;

        public ListaController(IListaService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListaRequestDTO dto)
        {
            var criada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id) =>
            Ok(await _service.GetByIdAsync(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ListaRequestDTO dto) =>
            Ok(await _service.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("professor/{professorId}")]
        public async Task<IActionResult> GetByProfessor(long professorId) =>
            Ok(await _service.GetByProfessorAsync(professorId));

        [HttpGet("turma/{turmaId}")]
        public async Task<IActionResult> GetByTurma(long turmaId) =>
            Ok(await _service.GetByTurmaAsync(turmaId));
    }
}
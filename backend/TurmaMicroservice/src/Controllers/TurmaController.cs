using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurmaAPI.DTOs;
using TurmaAPI.Services;

namespace TurmaAPI.Controllers
{
    /// <summary>Gerenciamento de turmas da plataforma.</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TurmaController : ControllerBase
    {
        private readonly ITurmaService _service;
        public TurmaController(ITurmaService service) => _service = service;

        // ── CRUD ─────────────────────────────────────────────────────────────

        /// <summary>Cadastra uma nova turma.</summary>
        /// <response code="201">Turma cadastrada com sucesso.</response>
        /// <response code="400">Já existe turma com esse nome, ano e turno.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TurmaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TurmaRequestDTO dto)
        {
            var criada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
        }

        /// <summary>Retorna todas as turmas com quantidade de alunos e professores.</summary>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TurmaResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        /// <summary>Busca uma turma pelo ID.</summary>
        /// <response code="200">Turma encontrada.</response>
        /// <response code="404">Turma não encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TurmaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id) =>
            Ok(await _service.GetByIdAsync(id));

        /// <summary>Atualiza os dados de uma turma.</summary>
        /// <response code="200">Turma atualizada com sucesso.</response>
        /// <response code="404">Turma não encontrada.</response>
        /// <response code="400">Dados inválidos ou turma duplicada.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TurmaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] TurmaRequestDTO dto) =>
            Ok(await _service.UpdateAsync(id, dto));

        /// <summary>Remove uma turma pelo ID.</summary>
        /// <response code="204">Turma removida com sucesso.</response>
        /// <response code="404">Turma não encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // ── Aluno_Turma ───────────────────────────────────────────────────────

        /// <summary>Matricula um aluno em uma turma.</summary>
        /// <response code="204">Aluno matriculado com sucesso.</response>
        /// <response code="400">Aluno já matriculado nessa turma.</response>
        /// <response code="404">Turma não encontrada.</response>
        [HttpPost("{turmaId}/matricular")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MatricularAluno(long turmaId, [FromBody] MatricularAlunoDTO dto)
        {
            await _service.MatricularAlunoAsync(turmaId, dto.AlunoId);
            return NoContent();
        }

        /// <summary>Desmatricula um aluno de uma turma.</summary>
        /// <response code="204">Aluno desmatriculado com sucesso.</response>
        /// <response code="404">Vínculo não encontrado.</response>
        [HttpDelete("{turmaId}/desmatricular/{alunoId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DesmatricularAluno(long turmaId, long alunoId)
        {
            await _service.DesmatricularAlunoAsync(turmaId, alunoId);
            return NoContent();
        }

        // ── Professor_Turma ───────────────────────────────────────────────────

        /// <summary>Vincula um professor a uma turma.</summary>
        /// <response code="204">Professor vinculado com sucesso.</response>
        /// <response code="400">Professor já vinculado a essa turma.</response>
        /// <response code="404">Turma não encontrada.</response>
        [HttpPost("{turmaId}/vincular-professor/{professorId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VincularProfessor(long turmaId, long professorId)
        {
            await _service.VincularProfessorAsync(turmaId, professorId);
            return NoContent();
        }

        /// <summary>Desvincula um professor de uma turma.</summary>
        /// <response code="204">Professor desvinculado com sucesso.</response>
        /// <response code="404">Vínculo não encontrado.</response>
        [HttpDelete("{turmaId}/desvincular-professor/{professorId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DesvincularProfessor(long turmaId, long professorId)
        {
            await _service.DesvincularProfessorAsync(turmaId, professorId);
            return NoContent();
        }
    }
}

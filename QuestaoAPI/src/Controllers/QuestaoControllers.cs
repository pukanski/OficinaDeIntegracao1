using Microsoft.AspNetCore.Mvc;
using QuestaoAPI.DTOs;
using QuestaoAPI.Services;

namespace QuestaoAPI.Controllers
{
    // ═══════════════════════════════════════════════════════════
    // PROVA
    // ═══════════════════════════════════════════════════════════

    /// <summary>Gerenciamento de provas de vestibulares e Enem.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProvaController : ControllerBase
    {
        private readonly IProvaService _service;
        public ProvaController(IProvaService service) => _service = service;

        /// <summary>Cadastra uma nova prova.</summary>
        /// <response code="201">Prova cadastrada com sucesso.</response>
        /// <response code="400">Prova já existe com esse vestibular, ano e edição.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProvaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProvaRequestDTO dto)
        {
            var criada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
        }

        /// <summary>Retorna todas as provas com total de questões.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProvaResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        /// <summary>Busca uma prova pelo ID.</summary>
        /// <response code="200">Prova encontrada.</response>
        /// <response code="404">Prova não encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProvaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id) =>
            Ok(await _service.GetByIdAsync(id));

        /// <summary>Atualiza uma prova.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProvaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] ProvaRequestDTO dto) =>
            Ok(await _service.UpdateAsync(id, dto));

        /// <summary>Remove uma prova e todas as suas questões.</summary>
        /// <response code="204">Prova removida com sucesso.</response>
        /// <response code="404">Prova não encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // QUESTAO
    // ═══════════════════════════════════════════════════════════

    /// <summary>Gerenciamento de questões da plataforma.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuestaoController : ControllerBase
    {
        private readonly IQuestaoService _service;
        public QuestaoController(IQuestaoService service) => _service = service;

        /// <summary>Cadastra uma nova questão.</summary>
        /// <response code="201">Questão cadastrada com sucesso.</response>
        /// <response code="400">Número de questão já existe nessa prova.</response>
        /// <response code="404">Prova não encontrada.</response>
        [HttpPost]
        [ProducesResponseType(typeof(QuestaoResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] QuestaoRequestDTO dto)
        {
            var criada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
        }

        /// <summary>Retorna todas as questões com alternativas.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<QuestaoResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        /// <summary>Retorna questões de uma prova específica.</summary>
        /// <response code="200">Questões retornadas com sucesso.</response>
        /// <response code="404">Prova não encontrada.</response>
        [HttpGet("prova/{provaId}")]
        [ProducesResponseType(typeof(List<QuestaoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByProva(long provaId) =>
            Ok(await _service.GetByProvaAsync(provaId));

        /// <summary>
        /// Filtra questões por disciplina, ano e/ou vestibular.
        /// Todos os parâmetros são opcionais e podem ser combinados.
        /// </summary>
        /// <param name="disciplina">Ex: Matemática</param>
        /// <param name="ano">Ex: 2024</param>
        /// <param name="vestibular">Ex: ENEM</param>
        [HttpGet("filtrar")]
        [ProducesResponseType(typeof(List<QuestaoResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByFiltro(
            [FromQuery] string? disciplina,
            [FromQuery] int?    ano,
            [FromQuery] string? vestibular) =>
                Ok(await _service.GetByFiltroAsync(disciplina, ano, vestibular));

        /// <summary>Busca uma questão pelo ID com suas alternativas.</summary>
        /// <response code="200">Questão encontrada.</response>
        /// <response code="404">Questão não encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(QuestaoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id) =>
            Ok(await _service.GetByIdAsync(id));

        /// <summary>Atualiza uma questão.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(QuestaoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] QuestaoRequestDTO dto) =>
            Ok(await _service.UpdateAsync(id, dto));

        /// <summary>Remove uma questão e suas alternativas.</summary>
        /// <response code="204">Questão removida com sucesso.</response>
        /// <response code="404">Questão não encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // ALTERNATIVA
    // ═══════════════════════════════════════════════════════════

    /// <summary>Gerenciamento de alternativas das questões.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AlternativaController : ControllerBase
    {
        private readonly IAlternativaService _service;
        public AlternativaController(IAlternativaService service) => _service = service;

        /// <summary>Cadastra uma alternativa para uma questão.</summary>
        /// <response code="201">Alternativa cadastrada com sucesso.</response>
        /// <response code="400">Letra duplicada ou questão já tem alternativa correta.</response>
        /// <response code="404">Questão não encontrada.</response>
        [HttpPost]
        [ProducesResponseType(typeof(AlternativaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] AlternativaRequestDTO dto)
        {
            var criada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
        }

        /// <summary>Retorna todas as alternativas de uma questão.</summary>
        /// <response code="200">Alternativas retornadas com sucesso.</response>
        /// <response code="404">Questão não encontrada.</response>
        [HttpGet("questao/{questaoId}")]
        [ProducesResponseType(typeof(List<AlternativaResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByQuestao(long questaoId) =>
            Ok(await _service.GetByQuestaoAsync(questaoId));

        /// <summary>Busca uma alternativa pelo ID.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AlternativaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id) =>
            Ok(await _service.GetByIdAsync(id));

        /// <summary>Atualiza uma alternativa.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AlternativaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] AlternativaRequestDTO dto) =>
            Ok(await _service.UpdateAsync(id, dto));

        /// <summary>Remove uma alternativa.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

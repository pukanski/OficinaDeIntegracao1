using Microsoft.AspNetCore.Mvc;
using DadosAPI.DTOs;
using DadosAPI.Services;

namespace DadosAPI.Controllers
{
    /// <summary>Registro de respostas e consulta de desempenho dos alunos.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DadosController : ControllerBase
    {
        private readonly IDadosService _service;
        public DadosController(IDadosService service) => _service = service;

        // ── Registro ──────────────────────────────────────────────────────────

        /// <summary>Registra a resposta de um aluno a uma questão.</summary>
        /// <response code="201">Resposta registrada com sucesso.</response>
        /// <response code="400">Aluno já respondeu essa questão.</response>
        [HttpPost("responder")]
        [ProducesResponseType(typeof(RespostaAlunoResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Responder([FromBody] RespostaAlunoRequestDTO dto)
        {
            var registrado = await _service.RegistrarRespostaAsync(dto);
            return CreatedAtAction(nameof(GetHistorico),
                new { alunoId = registrado.AlunoId }, registrado);
        }

        // ── Histórico ─────────────────────────────────────────────────────────

        /// <summary>
        /// Retorna o histórico paginado de respostas do aluno.
        /// </summary>
        /// <param name="alunoId">ID do aluno.</param>
        /// <param name="pagina">Número da página (padrão: 1).</param>
        /// <param name="tamanhoPagina">Registros por página, máximo 50 (padrão: 10).</param>
        /// <response code="200">Histórico retornado com sucesso.</response>
        /// <response code="404">Nenhuma resposta encontrada para esse aluno.</response>
        [HttpGet("{alunoId}/historico")]
        [ProducesResponseType(typeof(HistoricoRespostasDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHistorico(
            long alunoId,
            [FromQuery] int pagina        = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var historico = await _service.GetHistoricoAsync(alunoId, pagina, tamanhoPagina);
            return Ok(historico);
        }

        // ── Desempenho Geral ──────────────────────────────────────────────────

        /// <summary>Retorna o desempenho geral do aluno na plataforma.</summary>
        /// <param name="alunoId">ID do aluno.</param>
        /// <response code="200">Desempenho retornado com sucesso.</response>
        /// <response code="404">Nenhuma resposta encontrada para esse aluno.</response>
        [HttpGet("{alunoId}/desempenho")]
        [ProducesResponseType(typeof(DesempenhoGeralDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDesempenhoGeral(long alunoId) =>
            Ok(await _service.GetDesempenhoGeralAsync(alunoId));

        // ── Desempenho por Disciplina ─────────────────────────────────────────

        /// <summary>
        /// Retorna o desempenho do aluno agrupado por disciplina.
        /// Útil para o dashboard mostrar em quais matérias o aluno está bem ou mal.
        /// </summary>
        /// <param name="alunoId">ID do aluno.</param>
        /// <response code="200">Desempenho por disciplina retornado com sucesso.</response>
        /// <response code="404">Nenhuma resposta encontrada para esse aluno.</response>
        [HttpGet("{alunoId}/desempenho/disciplinas")]
        [ProducesResponseType(typeof(DesempenhoPorDisciplinaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDesempenhoPorDisciplina(long alunoId) =>
            Ok(await _service.GetDesempenhoPorDisciplinaAsync(alunoId));

        // ── Desempenho por Vestibular ─────────────────────────────────────────

        /// <summary>
        /// Retorna o desempenho do aluno agrupado por vestibular e ano.
        /// Útil para o dashboard mostrar como o aluno performa em cada prova.
        /// </summary>
        /// <param name="alunoId">ID do aluno.</param>
        /// <response code="200">Desempenho por vestibular retornado com sucesso.</response>
        /// <response code="404">Nenhuma resposta encontrada para esse aluno.</response>
        [HttpGet("{alunoId}/desempenho/vestibulares")]
        [ProducesResponseType(typeof(DesempenhoPorVestibularDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDesempenhoPorVestibular(long alunoId) =>
            Ok(await _service.GetDesempenhoPorVestibularAsync(alunoId));
    }
}

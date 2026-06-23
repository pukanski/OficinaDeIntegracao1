using AlunoAPI.Data;
using AlunoAPI.DTOs;
using AlunoAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlunoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Frequencia")]
    public class FrequenciaController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FrequenciaController(AppDbContext context) => _context = context;

        // POST /api/Frequencia/chamada — Salva ou sobrescreve uma chamada
        [HttpPost("chamada")]
        public async Task<IActionResult> SalvarChamada([FromBody] SalvarChamadaRequestDTO dto)
        {
            // Remove registros anteriores da mesma turma/data para permitir re-chamada
            var existentes = await _context.Frequencias
                .Where(f => f.TurmaId == dto.TurmaId && f.Data == dto.Data)
                .ToListAsync();
            _context.Frequencias.RemoveRange(existentes);

            foreach (var reg in dto.Registros)
            {
                _context.Frequencias.Add(new Frequencia
                {
                    TurmaId = dto.TurmaId,
                    AlunoId = reg.AlunoId,
                    Data = dto.Data,
                    Presente = reg.Presente
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Chamada salva com sucesso." });
        }

        // GET /api/Frequencia/historico/{turmaId} — Histórico de chamadas de uma turma
        [HttpGet("historico/{turmaId}")]
        public async Task<IActionResult> GetHistorico(long turmaId)
        {
            var historico = await _context.Frequencias
                .Where(f => f.TurmaId == turmaId)
                .GroupBy(f => f.Data)
                .Select(g => new HistoricoItemDTO
                {
                    TurmaId = turmaId,
                    Data = g.Key,
                    Presentes = g.Count(f => f.Presente),
                    Ausentes = g.Count(f => !f.Presente)
                })
                .OrderByDescending(h => h.Data)
                .ToListAsync();

            return Ok(historico);
        }

        // GET /api/Frequencia/chamada/{turmaId}/{data} — Detalhes de uma chamada específica
        [HttpGet("chamada/{turmaId}/{data}")]
        public async Task<IActionResult> GetChamada(long turmaId, DateOnly data)
        {
            var registros = await _context.Frequencias
                .Where(f => f.TurmaId == turmaId && f.Data == data)
                .ToListAsync();

            return Ok(new ChamadaResponseDTO
            {
                TurmaId = turmaId,
                Data = data,
                Presentes = registros.Count(f => f.Presente),
                Ausentes = registros.Count(f => !f.Presente),
                Registros = registros.Select(f => new FrequenciaAlunoResponseDTO
                {
                    AlunoId = f.AlunoId,
                    Presente = f.Presente
                }).ToList()
            });
        }

        // GET /api/Frequencia/percentual/{alunoId}/{turmaId} — Percentual de presença do aluno
        [HttpGet("percentual/{alunoId}/{turmaId}")]
        public async Task<IActionResult> GetPercentual(long alunoId, long turmaId)
        {
            var registros = await _context.Frequencias
                .Where(f => f.AlunoId == alunoId && f.TurmaId == turmaId)
                .ToListAsync();

            var total = registros.Count;
            var presencas = registros.Count(f => f.Presente);

            return Ok(new PercentualPresencaDTO
            {
                AlunoId = alunoId,
                TotalAulas = total,
                Presencas = presencas,
                Percentual = total > 0 ? Math.Round((double)presencas / total * 100, 1) : 0
            });
        }
    }
}

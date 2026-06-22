using Microsoft.EntityFrameworkCore;
using DadosAPI.Data;
using DadosAPI.DTOs;
using DadosAPI.Model;

namespace DadosAPI.Repositories
{
    public class DadosRepository : IDadosRepository
    {
        private readonly AppDbContext _context;

        public DadosRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── Registro ──────────────────────────────────────────────────────────

        public async Task<RespostaAluno> RegistrarRespostaAsync(RespostaAluno resposta)
        {
            _context.RespostasAlunos.Add(resposta);
            await _context.SaveChangesAsync();
            return resposta;
        }

        public async Task<bool> JaRespondeuAsync(long alunoId, long questaoId) =>
            await _context.RespostasAlunos
                .AnyAsync(r => r.AlunoId == alunoId && r.QuestaoId == questaoId);

        // ── Histórico ─────────────────────────────────────────────────────────

        public async Task<(List<RespostaAluno> respostas, int total)> GetHistoricoAsync(
            long alunoId, int pagina, int tamanhoPagina)
        {
            var query = _context.RespostasAlunos
                .Where(r => r.AlunoId == alunoId)
                .OrderByDescending(r => r.RespondidoEm);

            var total = await query.CountAsync();

            var respostas = await query
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (respostas, total);
        }

        // ── Desempenho Geral ──────────────────────────────────────────────────

        public async Task<DesempenhoGeralDTO> GetDesempenhoGeralAsync(long alunoId)
        {
            var respostas = await _context.RespostasAlunos
                .Where(r => r.AlunoId == alunoId)
                .ToListAsync();

            var total   = respostas.Count;
            var acertos = respostas.Count(r => r.Acertou);
            var erros   = total - acertos;

            return new DesempenhoGeralDTO
            {
                AlunoId           = alunoId,
                TotalRespondidas  = total,
                TotalAcertos      = acertos,
                TotalErros        = erros,
                PercentualAcerto  = total > 0
                    ? Math.Round((double)acertos / total * 100, 1)
                    : 0,
                PrimeiraResposta  = respostas.Any()
                    ? respostas.Min(r => r.RespondidoEm)
                    : null,
                UltimaResposta    = respostas.Any()
                    ? respostas.Max(r => r.RespondidoEm)
                    : null
            };
        }

        // ── Desempenho por Disciplina ─────────────────────────────────────────

        public async Task<List<DisciplinaDesempenhoDTO>> GetDesempenhoPorDisciplinaAsync(long alunoId)
        {
            var agrupado = await _context.RespostasAlunos
                .Where(r => r.AlunoId == alunoId)
                .GroupBy(r => r.Disciplina)
                .Select(g => new DisciplinaDesempenhoDTO
                {
                    Disciplina       = g.Key,
                    TotalRespondidas = g.Count(),
                    TotalAcertos     = g.Count(r => r.Acertou),
                    TotalErros       = g.Count(r => !r.Acertou),
                    PercentualAcerto = g.Count() > 0
                        ? Math.Round((double)g.Count(r => r.Acertou) / g.Count() * 100, 1)
                        : 0
                })
                .OrderByDescending(d => d.TotalRespondidas)
                .ToListAsync();

            return agrupado;
        }

        // ── Desempenho por Vestibular ─────────────────────────────────────────

        public async Task<List<VestibularDesempenhoDTO>> GetDesempenhoPorVestibularAsync(long alunoId)
        {
            var agrupado = await _context.RespostasAlunos
                .Where(r => r.AlunoId == alunoId)
                .GroupBy(r => new { r.Vestibular, r.AnoProva })
                .Select(g => new VestibularDesempenhoDTO
                {
                    Vestibular       = g.Key.Vestibular,
                    AnoProva         = g.Key.AnoProva,
                    TotalRespondidas = g.Count(),
                    TotalAcertos     = g.Count(r => r.Acertou),
                    TotalErros       = g.Count(r => !r.Acertou),
                    PercentualAcerto = g.Count() > 0
                        ? Math.Round((double)g.Count(r => r.Acertou) / g.Count() * 100, 1)
                        : 0
                })
                .OrderBy(v => v.Vestibular)
                .ThenByDescending(v => v.AnoProva)
                .ToListAsync();

            return agrupado;
        }
    }
}

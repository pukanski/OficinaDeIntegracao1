using DadosAPI.DTOs;
using DadosAPI.Model;

namespace DadosAPI.Repositories
{
    public interface IDadosRepository
    {
        // ── Registro ─────────────────────────────────────────────────────────
        Task<RespostaAluno> RegistrarRespostaAsync(RespostaAluno resposta);
        Task<bool> JaRespondeuAsync(long alunoId, long questaoId);

        // ── Histórico ────────────────────────────────────────────────────────
        Task<(List<RespostaAluno> respostas, int total)> GetHistoricoAsync(
            long alunoId, int pagina, int tamanhoPagina);

        // ── Desempenho Geral ─────────────────────────────────────────────────
        Task<DesempenhoGeralDTO> GetDesempenhoGeralAsync(long alunoId);

        // ── Desempenho por Disciplina ────────────────────────────────────────
        Task<List<DisciplinaDesempenhoDTO>> GetDesempenhoPorDisciplinaAsync(long alunoId);

        // ── Desempenho por Vestibular ────────────────────────────────────────
        Task<List<VestibularDesempenhoDTO>> GetDesempenhoPorVestibularAsync(long alunoId);
    }
}

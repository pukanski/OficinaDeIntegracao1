using QuestaoAPI.Model;

namespace QuestaoAPI.Repositories
{
    // ═══════════════════════════════════════════════════════════
    // PROVA
    // ═══════════════════════════════════════════════════════════
    public interface IProvaRepository
    {
        Task<Prova> CreateAsync(Prova prova);
        Task<List<(Prova prova, int totalQuestoes)>> GetAllAsync();
        Task<Prova?> GetByIdAsync(long id);
        Task<Prova?> UpdateAsync(long id, Prova prova);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExisteProvaAsync(string vestibular, int ano, string? edicao, long? ignorarId = null);
        Task<int> ContarQuestoesAsync(long provaId);
    }

    // ═══════════════════════════════════════════════════════════
    // QUESTAO
    // ═══════════════════════════════════════════════════════════
    public interface IQuestaoRepository
    {
        Task<Questao> CreateAsync(Questao questao);
        Task<List<Questao>> GetAllAsync();
        Task<List<Questao>> GetByProvaAsync(long provaId);
        Task<List<Questao>> GetByFiltroAsync(string? disciplina, int? ano, string? vestibular);
        Task<Questao?> GetByIdAsync(long id);
        Task<Questao?> UpdateAsync(long id, Questao questao);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExisteNumeroNaProvaAsync(long provaId, int numero, long? ignorarId = null);
    }

    // ═══════════════════════════════════════════════════════════
    // ALTERNATIVA
    // ═══════════════════════════════════════════════════════════
    public interface IAlternativaRepository
    {
        Task<Alternativa> CreateAsync(Alternativa alternativa);
        Task<List<Alternativa>> GetByQuestaoAsync(long questaoId);
        Task<Alternativa?> GetByIdAsync(long id);
        Task<Alternativa?> UpdateAsync(long id, Alternativa alternativa);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExisteLetraNaQuestaoAsync(long questaoId, string letra, long? ignorarId = null);
        Task<int> ContarCorretas(long questaoId);
    }
}

using QuestaoAPI.DTOs;

namespace QuestaoAPI.Services
{
    // ═══════════════════════════════════════════════════════════
    // PROVA
    // ═══════════════════════════════════════════════════════════
    public interface IProvaService
    {
        Task<ProvaResponseDTO> CreateAsync(ProvaRequestDTO dto);
        Task<List<ProvaResponseDTO>> GetAllAsync();
        Task<ProvaResponseDTO> GetByIdAsync(long id);
        Task<ProvaResponseDTO> UpdateAsync(long id, ProvaRequestDTO dto);
        Task<bool> DeleteAsync(long id);
    }

    // ═══════════════════════════════════════════════════════════
    // QUESTAO
    // ═══════════════════════════════════════════════════════════
    public interface IQuestaoService
    {
        Task<QuestaoResponseDTO> CreateAsync(QuestaoRequestDTO dto);
        Task<List<QuestaoResponseDTO>> GetAllAsync();
        Task<List<QuestaoResponseDTO>> GetByProvaAsync(long provaId);
        Task<List<QuestaoResponseDTO>> GetByFiltroAsync(string? disciplina, int? ano, string? vestibular);
        Task<QuestaoResponseDTO> GetByIdAsync(long id);
        Task<QuestaoResponseDTO> UpdateAsync(long id, QuestaoRequestDTO dto);
        Task<bool> DeleteAsync(long id);
    }

    // ═══════════════════════════════════════════════════════════
    // ALTERNATIVA
    // ═══════════════════════════════════════════════════════════
    public interface IAlternativaService
    {
        Task<AlternativaResponseDTO> CreateAsync(AlternativaRequestDTO dto);
        Task<List<AlternativaResponseDTO>> GetByQuestaoAsync(long questaoId);
        Task<AlternativaResponseDTO> GetByIdAsync(long id);
        Task<AlternativaResponseDTO> UpdateAsync(long id, AlternativaRequestDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}

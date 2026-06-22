using ListaAPI.Model;

namespace ListaAPI.Repositories
{
    public interface IListaRepository
    {
        Task<Lista> CreateAsync(Lista lista, List<long> questoesIds, List<long> turmasIds);
        Task<Lista?> GetByIdAsync(long id);
        Task<List<long>> GetQuestoesIdsAsync(long listaId);
        Task<List<long>> GetTurmasIdsAsync(long listaId);
        Task<Lista?> UpdateAsync(long id, Lista dadosNovos, List<long> questoesIds, List<long> turmasIds);
        Task<bool> DeleteAsync(long id);
        Task<List<Lista>> GetByProfessorAsync(long professorId);
        Task<List<long>> GetListasIdsByTurmaAsync(long turmaId);
    }
}
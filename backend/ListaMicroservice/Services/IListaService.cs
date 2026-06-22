using ListaAPI.DTOs;

namespace ListaAPI.Services
{
    public interface IListaService
    {
        Task<ListaResponseDTO> CreateAsync(ListaRequestDTO dto);
        Task<ListaResponseDTO> GetByIdAsync(long id);
        Task<ListaResponseDTO> UpdateAsync(long id, ListaRequestDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<List<ListaResponseDTO>> GetByProfessorAsync(long professorId);
        Task<List<ListaResponseDTO>> GetByTurmaAsync(long turmaId);
    }
}
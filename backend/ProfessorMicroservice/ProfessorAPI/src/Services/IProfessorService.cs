using ProfessorAPI.src.DTOs;

namespace ProfessorAPI.src.Services
{
    public interface IProfessorService
    {

        Task<ProfessorResponseDTO> CreateProfessorAsync(ProfessorRequestDTO professorRequest);
        Task<List<ProfessorResponseDTO>> GetAllProfessoresAsync();
        Task<ProfessorResponseDTO?> GetProfessorByIdAsync(long id);
        Task<ProfessorResponseDTO?> UpdateProfessorAsync(long id, ProfessorRequestDTO professorRequest);
        Task<bool> DeleteProfessorAsync(long id);

    }
}

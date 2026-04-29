using AlunoAPI.DTOs;
using AlunoAPI.Model;

namespace AlunoAPI.Services
{
    public interface IAlunoService
    {
        Task<AlunoResponseDTO> CreateAlunoAsync(AlunoRequestDTO dto);
        Task<List<AlunoResponseDTO>> GetAllAlunosAsync();
        Task<AlunoResponseDTO?> GetAlunoByIdAsync(long id);
        Task<AlunoResponseDTO> UpdateAlunoByIdAsync(long id, AlunoRequestDTO dto);
        Task<bool> DeleteAlunoByIdAsync(long id);

    }
}

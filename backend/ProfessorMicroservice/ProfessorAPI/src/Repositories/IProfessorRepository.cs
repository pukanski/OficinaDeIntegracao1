using ProfessorAPI.src.Model;

namespace ProfessorAPI.src.Repositories
{
    public interface IProfessorRepository
    {
        Task<Professor> CreateProfessorAsync(Professor professor);
        Task <List<Professor>> GetAllProfessoresAsync();
        Task<Professor?> GetProfessorByIdAsync(long id);
        Task<Professor?> UpdateProfessorAsync(long id, Professor professor);
        Task<bool> DeleteProfessorAsync(long id);

        //validações de unicidade
        //Task<bool> ExisteSiapeAsync(string siape, long? ignorarId = null);
        //Task<bool> ExisteEmailAsync(string email, long? ignorarId = null);


    }
}

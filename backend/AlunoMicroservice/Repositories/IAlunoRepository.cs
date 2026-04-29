using AlunoAPI.Model;

namespace AlunoAPI.Repositories
{
    public interface IAlunoRepository
    {
        Task<Aluno> CreateAlunoAsync(Aluno aluno);
        Task<List<Aluno>> GetAllAlunosAsync();
        Task<Aluno?> GetAlunoByIdAsync(long id);
        Task<Aluno?> UpdateAlunoByIdAsync(long id, Aluno aluno);
        Task<bool> DeleteAlunoByIdAsync(long id);
    }
}

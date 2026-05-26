using ProfessorAPI.src.DTOs;
using ProfessorAPI.src.Mappers;
using ProfessorAPI.src.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProfessorAPI.src.Services
{
    public class ProfessorService : IProfessorService
    {

        private readonly IProfessorRepository _repository;

        public ProfessorService(IProfessorRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProfessorResponseDTO> CreateProfessorAsync(ProfessorRequestDTO dto)
        {
            var professor = ProfessorMapper.ToModel(dto);

            try
            {
                var criado = await _repository.CreateProfessorAsync(professor);
                return ProfessorMapper.ToResponse(criado);
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Email ou SIAPE já cadastrados.");
            }
        }

        public async Task<List<ProfessorResponseDTO>> GetAllProfessoresAsync()
        {
            var professores = await _repository.GetAllProfessoresAsync();
            return ProfessorMapper.ToResponseList(professores);
        }

        public async Task<ProfessorResponseDTO?> GetProfessorByIdAsync(long id)
        {
            var professor = await _repository.GetProfessorByIdAsync(id);
            if (professor == null) throw new KeyNotFoundException($"Professor com ID {id} não encontrado.");
            
            return ProfessorMapper.ToResponse(professor);
        }

        public async Task<ProfessorResponseDTO> UpdateProfessorAsync(long id, ProfessorRequestDTO professorRequest)
        {

            var professor = await _repository.GetProfessorByIdAsync(id);
            if (professor == null) throw new KeyNotFoundException($"Professor com ID {id} não encontrado.");

            //validações de unicidade
            /*if (await _repository.ExisteSiapeAsync(professorRequest.siape, id))
                throw new InvalidOperationException("SIAPE já cadastrado.");

            if (await _repository.ExisteEmailAsync(professorRequest.email, id))
                throw new InvalidOperationException("EMAIL já cadastrado.");
            */
            var atualizadoProfessor = ProfessorMapper.ToModel(professorRequest);
            atualizadoProfessor.Id = id;
            var professorAtualizado = await _repository.UpdateProfessorAsync(id, atualizadoProfessor);
            return ProfessorMapper.ToResponse(professorAtualizado);
        }

        public async Task<bool> DeleteProfessorAsync(long id)
        {
            var deletado = await _repository.DeleteProfessorAsync(id);
            if (!deletado) throw new KeyNotFoundException ($"Professor com ID {id} não encontrado.");

            return true;
        }
    }
}

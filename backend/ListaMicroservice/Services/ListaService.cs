using ListaAPI.DTOs;
using ListaAPI.Mappers;
using ListaAPI.Repositories;

namespace ListaAPI.Services
{
    public class ListaService : IListaService
    {
        private readonly IListaRepository _repository;

        public ListaService(IListaRepository repository) => _repository = repository;

        public async Task<ListaResponseDTO> CreateAsync(ListaRequestDTO dto)
        {
            if (dto.QuestoesIds == null || !dto.QuestoesIds.Any())
                throw new ArgumentException("A lista deve conter pelo menos uma questão.");

            var listaModel = ListaMapper.ToModel(dto);
            var turmasSeguras = dto.TurmasIds ?? new List<long>();

            var listaCriada = await _repository.CreateAsync(listaModel, dto.QuestoesIds, turmasSeguras);

            return ListaMapper.ToResponse(listaCriada, dto.QuestoesIds, turmasSeguras);
        }

        public async Task<ListaResponseDTO> GetByIdAsync(long id)
        {
            var lista = await _repository.GetByIdAsync(id);
            if (lista == null)
                throw new KeyNotFoundException($"Lista com ID {id} não encontrada.");

            var questoesIds = await _repository.GetQuestoesIdsAsync(id);
            var turmasIds = await _repository.GetTurmasIdsAsync(id);

            return ListaMapper.ToResponse(lista, questoesIds, turmasIds);
        }

        public async Task<ListaResponseDTO> UpdateAsync(long id, ListaRequestDTO dto)
        {
            if (dto.QuestoesIds == null || !dto.QuestoesIds.Any())
                throw new ArgumentException("A lista deve conter pelo menos uma questão.");

            var listaModel = ListaMapper.ToModel(dto);
            var turmasSeguras = dto.TurmasIds ?? new List<long>();

            var atualizada = await _repository.UpdateAsync(id, listaModel, dto.QuestoesIds, turmasSeguras);

            if (atualizada == null)
                throw new KeyNotFoundException($"Lista com ID {id} não encontrada.");

            return ListaMapper.ToResponse(atualizada, dto.QuestoesIds, turmasSeguras);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var deletado = await _repository.DeleteAsync(id);
            if (!deletado)
                throw new KeyNotFoundException($"Lista com ID {id} não encontrada.");
            return true;
        }

        public async Task<List<ListaResponseDTO>> GetByProfessorAsync(long professorId)
        {
            var listas = await _repository.GetByProfessorAsync(professorId);
            var result = new List<ListaResponseDTO>();

            foreach (var lista in listas)
            {
                var questoes = await _repository.GetQuestoesIdsAsync(lista.Id);
                var turmas = await _repository.GetTurmasIdsAsync(lista.Id);
                result.Add(ListaMapper.ToResponse(lista, questoes, turmas));
            }
            return result;
        }

        public async Task<List<ListaResponseDTO>> GetByTurmaAsync(long turmaId)
        {
            var listasIds = await _repository.GetListasIdsByTurmaAsync(turmaId);
            var result = new List<ListaResponseDTO>();

            foreach (var id in listasIds)
            {
                result.Add(await GetByIdAsync(id));
            }
            return result;
        }
    }
}
using ListaAPI.DTOs;
using ListaAPI.Model;

namespace ListaAPI.Mappers
{
    public static class ListaMapper
    {
        public static Lista ToModel(ListaRequestDTO dto) => new Lista
        {
            Titulo = dto.Titulo.Trim(),
            ProfessorId = dto.ProfessorId,
            DataVencimento = dto.DataVencimento?.ToUniversalTime()
        };

        public static ListaResponseDTO ToResponse(Lista lista, List<long> questoesIds, List<long> turmasIds) => new ListaResponseDTO
        {
            Id = lista.Id,
            Titulo = lista.Titulo,
            ProfessorId = lista.ProfessorId,
            DataVencimento = lista.DataVencimento,
            QuestoesIds = questoesIds,
            TurmasIds = turmasIds,
            CriadoEm = lista.CriadoEm
        };
    }
}
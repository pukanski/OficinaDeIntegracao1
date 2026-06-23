using ProfessorAPI.src.DTOs;
using ProfessorAPI.src.Model;

namespace ProfessorAPI.src.Mappers
{
    public class ProfessorMapper
    {
        public static Professor ToModel(ProfessorRequestDTO dto)
        {
            return new Professor
            {
                AuthId = dto.AuthId,
                Siape = dto.Siape.Trim().ToUpper(),
                PrimeiroNome = dto.PrimeiroNome.Trim(),
                UltimoNome = dto.UltimoNome.Trim(),
                Email = dto.Email.Trim().ToLower(),
                Disciplina = dto.Disciplina.Trim()
            };
        }

        public static ProfessorResponseDTO ToResponse(Professor model)
        {
            return new ProfessorResponseDTO
            {
                Id = model.Id,
                AuthId = model.AuthId,
                Siape = model.Siape,
                PrimeiroNome = model.PrimeiroNome,
                UltimoNome = model.UltimoNome,
                Email = model.Email,
                Disciplina = model.Disciplina,
                CriadoEm = model.CriadoEm
            };
        }

        public static List<ProfessorResponseDTO> ToResponseList(List<Professor> professores) =>
            professores.Select(ToResponse).ToList();
    }
}
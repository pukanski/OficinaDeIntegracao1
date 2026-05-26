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

                Siape = dto.siape.Trim().ToUpper(),
                PrimeiroNome = dto.primeiroNome.Trim(),
                UltimoNome = dto.ultimoNome.Trim(),
                Email = dto.email.Trim().ToLower(),
                Disciplina = dto.disciplina.Trim(),

            };
        }


        public static ProfessorResponseDTO ToResponse(Professor model)
        {
            return new ProfessorResponseDTO
            {
                id = model.Id,
                siape = model.Siape,
                primeiroNome = model.PrimeiroNome,
                ultimoNome = model.UltimoNome,
                email = model.Email,
                disciplina = model.Disciplina,
                criadoEm = model.CriadoEm
            };
        }


        public static List<ProfessorResponseDTO> ToResponseList (List<Professor> professores) =>
            professores.Select(ToResponse).ToList();

    } 
}


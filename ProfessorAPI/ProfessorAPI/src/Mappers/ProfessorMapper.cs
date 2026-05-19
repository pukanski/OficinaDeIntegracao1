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

                siape = dto.siape.Trim().ToUpper(),
                primeiroNome = dto.primeiroNome.Trim(),
                ultimoNome = dto.ultimoNome.Trim(),
                email = dto.email.Trim().ToLower(),
                disciplina = dto.disciplina.Trim()

            };
        }


        public static ProfessorResponseDTO ToResponse(Professor model)
        {
            return new ProfessorResponseDTO
            {
                id = model.id,
                siape = model.siape,
                primeiroNome = model.primeiroNome,
                ultimoNome = model.ultimoNome,
                email = model.email,
                disciplina = model.disciplina,
                criadoEm = model.CriadoEm
            };
        }


        public static List<ProfessorResponseDTO> ToResponseList (List<Professor> professores) =>
            professores.Select(ToResponse).ToList();

    } 
}


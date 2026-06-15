using AlunoAPI.Model;
using AlunoAPI.DTOs;

namespace AlunoAPI.Mappers
{
    public class AlunoMapper
    {

        // RequestDto → Model (usado no Create e Update)
        public static Aluno ToModel(AlunoRequestDTO dto) {

            return new Aluno
            {
                PrimeiroNome = dto.PrimeiroNome.Trim(),
                UltimoNome = dto.UltimoNome.Trim(),
                Email = dto.Email.Trim().ToLower(),
                RA = dto.RA.Trim().ToUpper()

            };
        }
        // Model → ResponseDto (usado em todos os retornos)
        public static AlunoResponseDTO ToResponse(Aluno aluno) {

            return new AlunoResponseDTO
            {

                Id = aluno.Id,
                PrimeiroNome = aluno.PrimeiroNome,
                UltimoNome = aluno.UltimoNome,
                Email = aluno.Email,
                RA = aluno.RA,
                CriadoEm = aluno.CriadoEm,

            };
        }

        // Lista de Models → Lista de ResponseDtos
        public static List<AlunoResponseDTO> ToResponseList(List<Aluno> alunos) =>
            alunos.Select(ToResponse).ToList();
    }
}


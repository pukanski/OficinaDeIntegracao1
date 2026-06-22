using AlunoAPI.Model;
using AlunoAPI.DTOs;

namespace AlunoAPI.Mappers
{
    public class AlunoMapper
    {
        public static Aluno ToModel(AlunoRequestDTO dto)
        {
            return new Aluno
            {
                AuthId = dto.AuthId,
                PrimeiroNome = dto.PrimeiroNome.Trim(),
                UltimoNome = dto.UltimoNome.Trim(),
                Email = dto.Email.Trim().ToLower(),
                RA = dto.RA.Trim().ToUpper()
            };
        }

        public static AlunoResponseDTO ToResponse(Aluno aluno)
        {
            return new AlunoResponseDTO
            {
                Id = aluno.Id,
                AuthId = aluno.AuthId,
                PrimeiroNome = aluno.PrimeiroNome,
                UltimoNome = aluno.UltimoNome,
                Email = aluno.Email,
                RA = aluno.RA,
                CriadoEm = aluno.CriadoEm
            };
        }

        public static List<AlunoResponseDTO> ToResponseList(List<Aluno> alunos) =>
            alunos.Select(ToResponse).ToList();
    }
}
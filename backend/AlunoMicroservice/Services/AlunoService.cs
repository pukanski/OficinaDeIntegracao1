using AlunoAPI.DTOs;
using AlunoAPI.Mappers;
using AlunoAPI.Model;
using AlunoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlunoAPI.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;


        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<AlunoResponseDTO> CreateAlunoAsync(AlunoRequestDTO dto)
        {

            var aluno = AlunoMapper.ToModel(dto);
            var alunoCriado = await _alunoRepository.CreateAlunoAsync(aluno);
            return AlunoMapper.ToResponse(alunoCriado);
        }

        public async Task<List<AlunoResponseDTO>> GetAllAlunosAsync()
        {
            var alunos = await _alunoRepository.GetAllAlunosAsync();
            return AlunoMapper.ToResponseList(alunos);
        }

        public async Task<AlunoResponseDTO?> GetAlunoByIdAsync(long id)
        {
            var aluno = await _alunoRepository.GetAlunoByIdAsync(id);
            if (aluno == null)
            {
                return null;
            }
            return AlunoMapper.ToResponse(aluno);

        }

        public async Task<AlunoResponseDTO?> UpdateAlunoByIdAsync(long id, AlunoRequestDTO dto)
        {

            var alunoExistente = await _alunoRepository.GetAlunoByIdAsync(id);
            if(alunoExistente == null)
            {
                return null;
            }

            var dadosNovos = AlunoMapper.ToModel(dto);
            var atualizado = await _alunoRepository.UpdateAlunoByIdAsync(id, dadosNovos);

            return AlunoMapper.ToResponse(atualizado);

        }

        public async Task<bool> DeleteAlunoByIdAsync(long id)
        {
            var alunoExistente = await _alunoRepository.GetAlunoByIdAsync(id);
            if (alunoExistente == null)
            {
                return false;
            }
            return await _alunoRepository.DeleteAlunoByIdAsync(id);
        }
    }
}
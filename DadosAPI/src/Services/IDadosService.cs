using DadosAPI.DTOs;

namespace DadosAPI.Services
{
    public interface IDadosService
    {
        Task<RespostaAlunoResponseDTO> RegistrarRespostaAsync(RespostaAlunoRequestDTO dto);
        Task<HistoricoRespostasDTO> GetHistoricoAsync(long alunoId, int pagina, int tamanhoPagina);
        Task<DesempenhoGeralDTO> GetDesempenhoGeralAsync(long alunoId);
        Task<DesempenhoPorDisciplinaDTO> GetDesempenhoPorDisciplinaAsync(long alunoId);
        Task<DesempenhoPorVestibularDTO> GetDesempenhoPorVestibularAsync(long alunoId);
    }
}

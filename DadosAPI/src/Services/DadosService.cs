using DadosAPI.DTOs;
using DadosAPI.Mappers;
using DadosAPI.Repositories;

namespace DadosAPI.Services
{
    public class DadosService : IDadosService
    {
        private readonly IDadosRepository _repository;

        public DadosService(IDadosRepository repository)
        {
            _repository = repository;
        }

        // ── Registro ──────────────────────────────────────────────────────────

        public async Task<RespostaAlunoResponseDTO> RegistrarRespostaAsync(RespostaAlunoRequestDTO dto)
        {
            // Impede que o aluno responda a mesma questão duas vezes
            if (await _repository.JaRespondeuAsync(dto.AlunoId, dto.QuestaoId))
                throw new InvalidOperationException(
                    $"O aluno {dto.AlunoId} já respondeu a questão {dto.QuestaoId}.");

            var modelo   = DadosMapper.ToModel(dto);
            var registrado = await _repository.RegistrarRespostaAsync(modelo);
            return DadosMapper.ToResponse(registrado);
        }

        // ── Histórico ─────────────────────────────────────────────────────────

        public async Task<HistoricoRespostasDTO> GetHistoricoAsync(
            long alunoId, int pagina, int tamanhoPagina)
        {
            if (pagina < 1)        pagina        = 1;
            if (tamanhoPagina < 1) tamanhoPagina = 10;
            if (tamanhoPagina > 50) tamanhoPagina = 50; // limite máximo por página

            var (respostas, total) = await _repository.GetHistoricoAsync(alunoId, pagina, tamanhoPagina);

            return new HistoricoRespostasDTO
            {
                AlunoId       = alunoId,
                TotalRegistros = total,
                PaginaAtual   = pagina,
                TamanhoPagina = tamanhoPagina,
                TotalPaginas  = (int)Math.Ceiling((double)total / tamanhoPagina),
                Respostas     = DadosMapper.ToResponseList(respostas)
            };
        }

        // ── Desempenho Geral ──────────────────────────────────────────────────

        public async Task<DesempenhoGeralDTO> GetDesempenhoGeralAsync(long alunoId)
        {
            var desempenho = await _repository.GetDesempenhoGeralAsync(alunoId);

            if (desempenho.TotalRespondidas == 0)
                throw new KeyNotFoundException(
                    $"Nenhuma resposta encontrada para o aluno {alunoId}.");

            return desempenho;
        }

        // ── Desempenho por Disciplina ─────────────────────────────────────────

        public async Task<DesempenhoPorDisciplinaDTO> GetDesempenhoPorDisciplinaAsync(long alunoId)
        {
            var disciplinas = await _repository.GetDesempenhoPorDisciplinaAsync(alunoId);

            if (!disciplinas.Any())
                throw new KeyNotFoundException(
                    $"Nenhuma resposta encontrada para o aluno {alunoId}.");

            return new DesempenhoPorDisciplinaDTO
            {
                AlunoId     = alunoId,
                Disciplinas = disciplinas
            };
        }

        // ── Desempenho por Vestibular ─────────────────────────────────────────

        public async Task<DesempenhoPorVestibularDTO> GetDesempenhoPorVestibularAsync(long alunoId)
        {
            var vestibulares = await _repository.GetDesempenhoPorVestibularAsync(alunoId);

            if (!vestibulares.Any())
                throw new KeyNotFoundException(
                    $"Nenhuma resposta encontrada para o aluno {alunoId}.");

            return new DesempenhoPorVestibularDTO
            {
                AlunoId      = alunoId,
                Vestibulares = vestibulares
            };
        }
    }
}

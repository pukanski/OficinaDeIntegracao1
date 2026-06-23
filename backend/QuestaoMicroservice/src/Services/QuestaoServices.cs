using QuestaoAPI.DTOs;
using QuestaoAPI.Mappers;
using QuestaoAPI.Repositories;

namespace QuestaoAPI.Services
{
    // ═══════════════════════════════════════════════════════════
    // PROVA
    // ═══════════════════════════════════════════════════════════
    public class ProvaService : IProvaService
    {
        private readonly IProvaRepository _repository;
        public ProvaService(IProvaRepository repository) => _repository = repository;

        public async Task<ProvaResponseDTO> CreateAsync(ProvaRequestDTO dto)
        {
            if (await _repository.ExisteProvaAsync(dto.Vestibular.ToUpper(), dto.Ano, dto.Edicao))
                throw new InvalidOperationException("Já existe uma prova com esse vestibular, ano e edição.");

            var prova = ProvaMapper.ToModel(dto);
            var criada = await _repository.CreateAsync(prova);
            return ProvaMapper.ToResponse(criada);
        }

        public async Task<List<ProvaResponseDTO>> GetAllAsync()
        {
            var provas = await _repository.GetAllAsync();
            return ProvaMapper.ToResponseList(provas);
        }

        public async Task<ProvaResponseDTO> GetByIdAsync(long id)
        {
            var prova = await _repository.GetByIdAsync(id);
            if (prova == null)
                throw new KeyNotFoundException($"Prova com ID {id} não encontrada.");

            var total = await _repository.ContarQuestoesAsync(id);
            return ProvaMapper.ToResponse(prova, total);
        }

        public async Task<ProvaResponseDTO> UpdateAsync(long id, ProvaRequestDTO dto)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"Prova com ID {id} não encontrada.");

            if (await _repository.ExisteProvaAsync(dto.Vestibular.ToUpper(), dto.Ano, dto.Edicao, ignorarId: id))
                throw new InvalidOperationException("Já existe outra prova com esse vestibular, ano e edição.");

            var dadosNovos = ProvaMapper.ToModel(dto);
            var atualizada = await _repository.UpdateAsync(id, dadosNovos);
            return ProvaMapper.ToResponse(atualizada!);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"Prova com ID {id} não encontrada.");

            return await _repository.DeleteAsync(id);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // QUESTAO
    // ═══════════════════════════════════════════════════════════
    public class QuestaoService : IQuestaoService
    {
        private readonly IQuestaoRepository _repository;
        private readonly IProvaRepository _provaRepository;
        private readonly IAlternativaRepository _alternativaRepository; // NOVA INJEÇÃO

        // Atualize o construtor para receber o repositório de alternativas
        public QuestaoService(
            IQuestaoRepository repository,
            IProvaRepository provaRepository,
            IAlternativaRepository alternativaRepository)
        {
            _repository = repository;
            _provaRepository = provaRepository;
            _alternativaRepository = alternativaRepository;
        }

        public async Task<QuestaoResponseDTO> CreateAsync(QuestaoRequestDTO dto)
        {
            // Verifica prova apenas se ProvaId foi informado
            if (dto.ProvaId.HasValue)
            {
                var prova = await _provaRepository.GetByIdAsync(dto.ProvaId.Value);
                if (prova == null)
                    throw new KeyNotFoundException($"Prova com ID {dto.ProvaId} não encontrada.");

                if (await _repository.ExisteNumeroNaProvaAsync(dto.ProvaId.Value, dto.Numero))
                    throw new InvalidOperationException($"Já existe a questão de número {dto.Numero} nessa prova.");
            }

            // 1. Salva a questão principal no banco e gera o ID
            var questao = QuestaoMapper.ToModel(dto);
            var criada = await _repository.CreateAsync(questao);

            // 2. Transação vinculada: Salva as alternativas
            if (dto.Alternativas != null && dto.Alternativas.Any())
            {
                foreach (var altDto in dto.Alternativas)
                {
                    altDto.QuestaoId = criada.Id; // Vincula a alternativa à questão recém-nascida
                    var alternativa = AlternativaMapper.ToModel(altDto);
                    await _alternativaRepository.CreateAsync(alternativa);
                }
            }

            // 3. Recarrega com includes (agora com as alternativas no banco) para o response completo
            var completa = await _repository.GetByIdAsync(criada.Id);
            return QuestaoMapper.ToResponse(completa!);
        }

        public async Task<List<QuestaoResponseDTO>> GetAllAsync()
        {
            var questoes = await _repository.GetAllAsync();
            return QuestaoMapper.ToResponseList(questoes);
        }

        public async Task<List<QuestaoResponseDTO>> GetByProvaAsync(long provaId)
        {
            var prova = await _provaRepository.GetByIdAsync(provaId);
            if (prova == null)
                throw new KeyNotFoundException($"Prova com ID {provaId} não encontrada.");

            var questoes = await _repository.GetByProvaAsync(provaId);
            return QuestaoMapper.ToResponseList(questoes);
        }

        public async Task<List<QuestaoResponseDTO>> GetByFiltroAsync(string? disciplina, int? ano, string? vestibular)
        {
            var questoes = await _repository.GetByFiltroAsync(disciplina, ano, vestibular);
            return QuestaoMapper.ToResponseList(questoes);
        }

        public async Task<QuestaoResponseDTO> GetByIdAsync(long id)
        {
            var questao = await _repository.GetByIdAsync(id);
            if (questao == null)
                throw new KeyNotFoundException($"Questão com ID {id} não encontrada.");

            return QuestaoMapper.ToResponse(questao);
        }

        public async Task<QuestaoResponseDTO> UpdateAsync(long id, QuestaoRequestDTO dto)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"Questão com ID {id} não encontrada.");

            if (dto.ProvaId.HasValue)
            {
                var prova = await _provaRepository.GetByIdAsync(dto.ProvaId.Value);
                if (prova == null)
                    throw new KeyNotFoundException($"Prova com ID {dto.ProvaId} não encontrada.");

                if (await _repository.ExisteNumeroNaProvaAsync(dto.ProvaId.Value, dto.Numero, ignorarId: id))
                    throw new InvalidOperationException($"Já existe outra questão de número {dto.Numero} nessa prova.");
            }

            var dadosNovos = QuestaoMapper.ToModel(dto);
            var atualizada = await _repository.UpdateAsync(id, dadosNovos);

            var completa = await _repository.GetByIdAsync(atualizada!.Id);
            return QuestaoMapper.ToResponse(completa!);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"Questão com ID {id} não encontrada.");

            return await _repository.DeleteAsync(id);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // ALTERNATIVA
    // ═══════════════════════════════════════════════════════════
    public class AlternativaService : IAlternativaService
    {
        private readonly IAlternativaRepository _repository;
        private readonly IQuestaoRepository _questaoRepository;

        public AlternativaService(IAlternativaRepository repository, IQuestaoRepository questaoRepository)
        {
            _repository = repository;
            _questaoRepository = questaoRepository;
        }

        public async Task<AlternativaResponseDTO> CreateAsync(AlternativaRequestDTO dto)
        {
            // Verifica se a questão existe
            var questao = await _questaoRepository.GetByIdAsync(dto.QuestaoId);
            if (questao == null)
                throw new KeyNotFoundException($"Questão com ID {dto.QuestaoId} não encontrada.");

            // Verifica letra duplicada
            if (await _repository.ExisteLetraNaQuestaoAsync(dto.QuestaoId, dto.Letra.ToUpper()))
                throw new InvalidOperationException($"Já existe uma alternativa com a letra {dto.Letra} nessa questão.");

            // Garante que só uma alternativa é marcada como correta
            if (dto.Correta)
            {
                var corretas = await _repository.ContarCorretas(dto.QuestaoId);
                if (corretas > 0)
                    throw new InvalidOperationException("Essa questão já possui uma alternativa correta.");
            }

            var alternativa = AlternativaMapper.ToModel(dto);
            var criada = await _repository.CreateAsync(alternativa);
            return AlternativaMapper.ToResponse(criada);
        }

        public async Task<List<AlternativaResponseDTO>> GetByQuestaoAsync(long questaoId)
        {
            var questao = await _questaoRepository.GetByIdAsync(questaoId);
            if (questao == null)
                throw new KeyNotFoundException($"Questão com ID {questaoId} não encontrada.");

            var alternativas = await _repository.GetByQuestaoAsync(questaoId);
            return AlternativaMapper.ToResponseList(alternativas);
        }

        public async Task<AlternativaResponseDTO> GetByIdAsync(long id)
        {
            var alternativa = await _repository.GetByIdAsync(id);
            if (alternativa == null)
                throw new KeyNotFoundException($"Alternativa com ID {id} não encontrada.");

            return AlternativaMapper.ToResponse(alternativa);
        }

        public async Task<AlternativaResponseDTO> UpdateAsync(long id, AlternativaRequestDTO dto)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"Alternativa com ID {id} não encontrada.");

            if (await _repository.ExisteLetraNaQuestaoAsync(dto.QuestaoId, dto.Letra.ToUpper(), ignorarId: id))
                throw new InvalidOperationException($"Já existe outra alternativa com a letra {dto.Letra} nessa questão.");

            if (dto.Correta && !existe.Correta)
            {
                var corretas = await _repository.ContarCorretas(dto.QuestaoId);
                if (corretas > 0)
                    throw new InvalidOperationException("Essa questão já possui uma alternativa correta.");
            }

            var dadosNovos = AlternativaMapper.ToModel(dto);
            var atualizada = await _repository.UpdateAsync(id, dadosNovos);
            return AlternativaMapper.ToResponse(atualizada!);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"Alternativa com ID {id} não encontrada.");

            return await _repository.DeleteAsync(id);
        }
    }
}

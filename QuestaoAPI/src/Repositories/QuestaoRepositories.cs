using Microsoft.EntityFrameworkCore;
using QuestaoAPI.Data;
using QuestaoAPI.Model;

namespace QuestaoAPI.Repositories
{
    // ═══════════════════════════════════════════════════════════
    // PROVA
    // ═══════════════════════════════════════════════════════════
    public class ProvaRepository : IProvaRepository
    {
        private readonly AppDbContext _context;
        public ProvaRepository(AppDbContext context) => _context = context;

        public async Task<Prova> CreateAsync(Prova prova)
        {
            _context.Provas.Add(prova);
            await _context.SaveChangesAsync();
            return prova;
        }

        public async Task<List<(Prova prova, int totalQuestoes)>> GetAllAsync()
        {
            var provas = await _context.Provas
                .OrderBy(p => p.Vestibular)
                .ThenBy(p => p.Ano)
                .ToListAsync();

            var resultado = new List<(Prova, int)>();
            foreach (var prova in provas)
            {
                var total = await ContarQuestoesAsync(prova.Id);
                resultado.Add((prova, total));
            }
            return resultado;
        }

        public async Task<Prova?> GetByIdAsync(long id) =>
            await _context.Provas.FindAsync(id);

        public async Task<Prova?> UpdateAsync(long id, Prova dadosNovos)
        {
            var prova = await _context.Provas.FindAsync(id);
            if (prova == null) return null;

            prova.Vestibular   = dadosNovos.Vestibular;
            prova.Ano          = dadosNovos.Ano;
            prova.Edicao       = dadosNovos.Edicao;
            prova.AtualizadoEm = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return prova;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var prova = await _context.Provas.FindAsync(id);
            if (prova == null) return false;

            _context.Provas.Remove(prova);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteProvaAsync(string vestibular, int ano, string? edicao, long? ignorarId = null) =>
            await _context.Provas.AnyAsync(p =>
                p.Vestibular == vestibular &&
                p.Ano        == ano        &&
                p.Edicao     == edicao     &&
                (ignorarId == null || p.Id != ignorarId));

        public async Task<int> ContarQuestoesAsync(long provaId) =>
            await _context.Questoes.CountAsync(q => q.ProvaId == provaId);
    }

    // ═══════════════════════════════════════════════════════════
    // QUESTAO
    // ═══════════════════════════════════════════════════════════
    public class QuestaoRepository : IQuestaoRepository
    {
        private readonly AppDbContext _context;
        public QuestaoRepository(AppDbContext context) => _context = context;

        public async Task<Questao> CreateAsync(Questao questao)
        {
            _context.Questoes.Add(questao);
            await _context.SaveChangesAsync();
            return questao;
        }

        public async Task<List<Questao>> GetAllAsync() =>
            await _context.Questoes
                .Include(q => q.Prova)
                .Include(q => q.Alternativas)
                .OrderBy(q => q.ProvaId)
                .ThenBy(q => q.Numero)
                .ToListAsync();

        public async Task<List<Questao>> GetByProvaAsync(long provaId) =>
            await _context.Questoes
                .Include(q => q.Prova)
                .Include(q => q.Alternativas)
                .Where(q => q.ProvaId == provaId)
                .OrderBy(q => q.Numero)
                .ToListAsync();

        public async Task<List<Questao>> GetByFiltroAsync(string? disciplina, int? ano, string? vestibular) =>
            await _context.Questoes
                .Include(q => q.Prova)
                .Include(q => q.Alternativas)
                .Where(q =>
                    (disciplina  == null || q.Disciplina         == disciplina)  &&
                    (ano         == null || q.Prova.Ano          == ano)         &&
                    (vestibular  == null || q.Prova.Vestibular   == vestibular.ToUpper()))
                .OrderBy(q => q.Prova.Ano)
                .ThenBy(q => q.Numero)
                .ToListAsync();

        public async Task<Questao?> GetByIdAsync(long id) =>
            await _context.Questoes
                .Include(q => q.Prova)
                .Include(q => q.Alternativas)
                .FirstOrDefaultAsync(q => q.Id == id);

        public async Task<Questao?> UpdateAsync(long id, Questao dadosNovos)
        {
            var questao = await _context.Questoes.FindAsync(id);
            if (questao == null) return null;

            questao.ProvaId     = dadosNovos.ProvaId;
            questao.Disciplina  = dadosNovos.Disciplina;
            questao.Materia     = dadosNovos.Materia;
            questao.Enunciado   = dadosNovos.Enunciado;
            questao.Dificuldade = dadosNovos.Dificuldade;
            questao.Numero      = dadosNovos.Numero;
            questao.AtualizadoEm = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return questao;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var questao = await _context.Questoes.FindAsync(id);
            if (questao == null) return false;

            _context.Questoes.Remove(questao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteNumeroNaProvaAsync(long provaId, int numero, long? ignorarId = null) =>
            await _context.Questoes.AnyAsync(q =>
                q.ProvaId == provaId &&
                q.Numero  == numero  &&
                (ignorarId == null || q.Id != ignorarId));
    }

    // ═══════════════════════════════════════════════════════════
    // ALTERNATIVA
    // ═══════════════════════════════════════════════════════════
    public class AlternativaRepository : IAlternativaRepository
    {
        private readonly AppDbContext _context;
        public AlternativaRepository(AppDbContext context) => _context = context;

        public async Task<Alternativa> CreateAsync(Alternativa alternativa)
        {
            _context.Alternativas.Add(alternativa);
            await _context.SaveChangesAsync();
            return alternativa;
        }

        public async Task<List<Alternativa>> GetByQuestaoAsync(long questaoId) =>
            await _context.Alternativas
                .Where(a => a.QuestaoId == questaoId)
                .OrderBy(a => a.Letra)
                .ToListAsync();

        public async Task<Alternativa?> GetByIdAsync(long id) =>
            await _context.Alternativas.FindAsync(id);

        public async Task<Alternativa?> UpdateAsync(long id, Alternativa dadosNovos)
        {
            var alternativa = await _context.Alternativas.FindAsync(id);
            if (alternativa == null) return null;

            alternativa.Letra        = dadosNovos.Letra;
            alternativa.Texto        = dadosNovos.Texto;
            alternativa.Correta      = dadosNovos.Correta;
            alternativa.AtualizadoEm = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return alternativa;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var alternativa = await _context.Alternativas.FindAsync(id);
            if (alternativa == null) return false;

            _context.Alternativas.Remove(alternativa);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteLetraNaQuestaoAsync(long questaoId, string letra, long? ignorarId = null) =>
            await _context.Alternativas.AnyAsync(a =>
                a.QuestaoId == questaoId &&
                a.Letra     == letra     &&
                (ignorarId == null || a.Id != ignorarId));

        public async Task<int> ContarCorretas(long questaoId) =>
            await _context.Alternativas
                .CountAsync(a => a.QuestaoId == questaoId && a.Correta);
    }
}

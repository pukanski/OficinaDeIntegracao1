using Microsoft.EntityFrameworkCore;
using ListaAPI.Data;
using ListaAPI.Model;

namespace ListaAPI.Repositories
{
    public class ListaRepository : IListaRepository
    {
        private readonly AppDbContext _context;

        public ListaRepository(AppDbContext context) => _context = context;

        public async Task<Lista> CreateAsync(Lista lista, List<long> questoesIds, List<long> turmasIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Listas.Add(lista);
                await _context.SaveChangesAsync();

                var listaQuestoes = questoesIds.Distinct().Select(qId => new ListaQuestao
                {
                    ListaId = lista.Id,
                    QuestaoId = qId
                });
                _context.ListaQuestoes.AddRange(listaQuestoes);

                var listaTurmas = turmasIds.Distinct().Select(tId => new ListaTurma
                {
                    ListaId = lista.Id,
                    TurmaId = tId
                });
                _context.ListaTurmas.AddRange(listaTurmas);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return lista;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Lista?> GetByIdAsync(long id) =>
            await _context.Listas.FindAsync(id);

        public async Task<List<long>> GetQuestoesIdsAsync(long listaId) =>
            await _context.ListaQuestoes.Where(lq => lq.ListaId == listaId).Select(lq => lq.QuestaoId).ToListAsync();

        public async Task<List<long>> GetTurmasIdsAsync(long listaId) =>
            await _context.ListaTurmas.Where(lt => lt.ListaId == listaId).Select(lt => lt.TurmaId).ToListAsync();

        public async Task<Lista?> UpdateAsync(long id, Lista dadosNovos, List<long> questoesIds, List<long> turmasIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var lista = await _context.Listas.FindAsync(id);
                if (lista == null) return null;

                // Atualiza dados base
                lista.Titulo = dadosNovos.Titulo;
                lista.DataVencimento = dadosNovos.DataVencimento;
                lista.AtualizadoEm = DateTime.UtcNow;

                // Varre e destrói vínculos antigos (Substituição Total)
                var antigasQuestoes = _context.ListaQuestoes.Where(lq => lq.ListaId == id);
                _context.ListaQuestoes.RemoveRange(antigasQuestoes);

                var antigasTurmas = _context.ListaTurmas.Where(lt => lt.ListaId == id);
                _context.ListaTurmas.RemoveRange(antigasTurmas);

                await _context.SaveChangesAsync(); // Efetiva deleção para liberar as chaves únicas

                // Recria vínculos
                var novasQuestoes = questoesIds.Distinct().Select(qId => new ListaQuestao { ListaId = id, QuestaoId = qId });
                _context.ListaQuestoes.AddRange(novasQuestoes);

                var novasTurmas = turmasIds.Distinct().Select(tId => new ListaTurma { ListaId = id, TurmaId = tId });
                _context.ListaTurmas.AddRange(novasTurmas);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return lista;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var lista = await _context.Listas.FindAsync(id);
            if (lista == null) return false;

            _context.Listas.Remove(lista);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Lista>> GetByProfessorAsync(long professorId) =>
            await _context.Listas.Where(l => l.ProfessorId == professorId).ToListAsync();

        public async Task<List<long>> GetListasIdsByTurmaAsync(long turmaId) =>
            await _context.ListaTurmas.Where(lt => lt.TurmaId == turmaId).Select(lt => lt.ListaId).ToListAsync();
    }
}
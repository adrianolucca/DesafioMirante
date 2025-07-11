using Domain.Entities;
using Domain.Interfaces;
using DesafioMirante.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> ObterTodasAsync()
        {
            return await _context.Tarefas
                .Include(t => t.Status)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Tarefa?> ObterPorIdAsync(Guid id)
        {
            return await _context.Tarefas
                .Include(t => t.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tarefa>> FiltrarAsync(int? statusId, DateTime? dataVencimento)
        {
            var query = _context.Tarefas.Include(t => t.Status).AsQueryable();

            if (statusId.HasValue)
                query = query.Where(t => t.StatusId == statusId.Value);

            if (dataVencimento.HasValue)
                query = query.Where(t => t.DataVencimento.Date == dataVencimento.Value.Date);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task AdicionarAsync(Tarefa tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
        }

        public async Task AtualizarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
        }

        public async Task RemoverAsync(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
        }
    }
}

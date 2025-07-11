using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> ObterTodasAsync();
        Task<Tarefa?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Tarefa>> FiltrarAsync(int? statusId, DateTime? dataVencimento);
        Task AdicionarAsync(Tarefa tarefa);
        Task AtualizarAsync(Tarefa tarefa);
        Task RemoverAsync(Tarefa tarefa);
    }
}

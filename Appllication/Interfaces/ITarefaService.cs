using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITarefaService
    {
        Task<IEnumerable<TarefaResponseDto>> ObterTodasAsync();
        Task<TarefaDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<TarefaDto>> FiltrarAsync(int? statusId, DateTime? dataVencimento);
        Task AdicionarAsync(TarefaDto tarefa);
        Task AtualizarAsync(TarefaDto tarefa);
        Task RemoverAsync(Guid id);
    }
}

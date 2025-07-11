using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace DesafioMirante.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TarefaService(
            ITarefaRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TarefaResponseDto>> ObterTodasAsync()
        {
            var tarefas = await _repository.ObterTodasAsync();
            return _mapper.Map<IEnumerable<TarefaResponseDto>>(tarefas);
        }

        public async Task<TarefaDto?> ObterPorIdAsync(Guid id)
        {
            var tarefa = await _repository.ObterPorIdAsync(id);
            return _mapper.Map<TarefaDto>(tarefa);
        }

        public async Task<IEnumerable<TarefaDto>> FiltrarAsync(int? statusId, DateTime? dataVencimento)
        {
            var tarefas = await _repository.FiltrarAsync(statusId, dataVencimento);
            return _mapper.Map<IEnumerable<TarefaDto>>(tarefas);
        }

        public async Task AdicionarAsync(TarefaDto dto)
        {
            try
            {
                var entidade = _mapper.Map<Tarefa>(dto);
                await _repository.AdicionarAsync(entidade);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
            }
        }

        public async Task AtualizarAsync(TarefaDto dto)
        {
            try
            {
                var entidade = _mapper.Map<Tarefa>(dto);
                await _repository.AtualizarAsync(entidade);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
            }
        }

        public async Task RemoverAsync(Guid id)
        {
            try
            {
                var tarefa = await _repository.ObterPorIdAsync(id);
                if (tarefa != null)
                {
                    await _repository.RemoverAsync(tarefa);
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
            }
        }
    }
}

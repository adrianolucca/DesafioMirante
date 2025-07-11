using Xunit;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using Domain.Interfaces;
using DesafioMirante.Application.Services;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

public class TarefaServiceTests
{
    private readonly Mock<ITarefaRepository> _tarefaRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly ITarefaService _tarefaService;

    public TarefaServiceTests()
    {
        _tarefaRepoMock = new Mock<ITarefaRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TarefaDto, Tarefa>();
        });

        _mapper = config.CreateMapper();

        _tarefaService = new TarefaService(_tarefaRepoMock.Object, _unitOfWorkMock.Object, _mapper);
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarTarefaEChamarCommit()
    {
        // Arrange
        var dto = new TarefaDto { Titulo = "Testando Tarefa", StatusId = 1 };

        // Act
        await _tarefaService.AdicionarAsync(dto);

        // Assert
        _tarefaRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Tarefa>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }
}

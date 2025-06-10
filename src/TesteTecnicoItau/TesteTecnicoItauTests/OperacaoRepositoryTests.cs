using Microsoft.EntityFrameworkCore;
using Moq;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories;

public class OperacaoRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger> _mockLogger;
    private readonly OperacaoRepository _repository;

    public OperacaoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _mockLogger = new Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger>();
        _repository = new OperacaoRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarOperacao()
    {
        var operacao = new Operacao
        {
            UsuarioId = 1,
            AtivoId = 2,
            DataHora = DateTime.Now
        };

        await _repository.AddAsync(operacao);

        var operacoes = await _context.Operacoes.ToListAsync();
        Assert.Single(operacoes);
        Assert.Equal(1, operacoes[0].UsuarioId);
        Assert.Equal(2, operacoes[0].AtivoId);

        // Verifica se o LogInfo foi chamado (opcional)
        _mockLogger.Verify(
            x => x.LogInfo(It.Is<string>(s => s.Contains("Operação adicionada"))),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarOperacao_QuandoExiste()
    {
        var operacao = new Operacao { Id = 1, UsuarioId = 10, AtivoId = 20 };
        await _context.Operacoes.AddAsync(operacao);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(10, result.UsuarioId);
        Assert.Equal(20, result.AtivoId);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        var result = await _repository.GetByIdAsync(999);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodasOperacoes()
    {
        await _context.Operacoes.AddRangeAsync(
            new Operacao { Id = 1, UsuarioId = 1, AtivoId = 1 },
            new Operacao { Id = 2, UsuarioId = 2, AtivoId = 2 }
        );
        await _context.SaveChangesAsync();

        var operacoes = await _repository.GetAllAsync();

        Assert.NotNull(operacoes);
        Assert.Equal(2, operacoes.Count());
    }

    [Fact]
    public async Task GetByUsuarioAtivoAsync_DeveRetornarOperacoesCorretas()
    {
        await _context.Operacoes.AddRangeAsync(
            new Operacao { Id = 1, UsuarioId = 1, AtivoId = 1 },
            new Operacao { Id = 2, UsuarioId = 1, AtivoId = 2 },
            new Operacao { Id = 3, UsuarioId = 2, AtivoId = 1 }
        );
        await _context.SaveChangesAsync();

        var resultado = await _repository.GetByUsuarioAtivoAsync(1, 1);

        Assert.Single(resultado);
        Assert.All(resultado, o => Assert.Equal(1, o.UsuarioId));
        Assert.All(resultado, o => Assert.Equal(1, o.AtivoId));
    }

    [Fact]
    public async Task GetRecentesPorUsuarioAtivoAsync_DeveRetornarOperacoesRecentes()
    {
        var dataLimite = DateTime.Now.AddDays(-1);

        await _context.Operacoes.AddRangeAsync(
            new Operacao { Id = 1, UsuarioId = 1, AtivoId = 1, DataHora = DateTime.Now.AddHours(-2) },
            new Operacao { Id = 2, UsuarioId = 1, AtivoId = 1, DataHora = DateTime.Now.AddDays(-2) },
            new Operacao { Id = 3, UsuarioId = 2, AtivoId = 1, DataHora = DateTime.Now }
        );
        await _context.SaveChangesAsync();

        var resultado = await _repository.GetRecentesPorUsuarioAtivoAsync(1, 1, dataLimite);

        Assert.Single(resultado);
        Assert.All(resultado, o => Assert.True(o.DataHora >= dataLimite));
        Assert.All(resultado, o => Assert.Equal(1, o.UsuarioId));
        Assert.All(resultado, o => Assert.Equal(1, o.AtivoId));
    }

    [Fact]
    public async Task AddAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Operacoes.AddAsync(It.IsAny<Operacao>(), default))
                   .Throws(new Exception("Erro simulado"));

        var repo = new OperacaoRepository(mockContext.Object, _mockLogger.Object);

        var operacao = new Operacao { UsuarioId = 1, AtivoId = 2, DataHora = DateTime.Now };

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.AddAsync(operacao));
        Assert.Equal("Erro ao adicionar operação.", ex.Message);
    }
}
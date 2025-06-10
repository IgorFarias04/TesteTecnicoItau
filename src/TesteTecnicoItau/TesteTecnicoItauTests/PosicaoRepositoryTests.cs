using Microsoft.EntityFrameworkCore;
using Moq;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories;

public class PosicaoRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger> _mockLogger;
    private readonly PosicaoRepository _repository;

    public PosicaoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _mockLogger = new Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger>();
        _repository = new PosicaoRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarPosicao()
    {
        var posicao = new Posicao
        {
            UsuarioId = 1,
            AtivoId = 2,
            Qtd = 100,
            PrecoMedio = 50.5m,
            Pl = 5000m
        };

        await _repository.AddAsync(posicao);

        var posicoes = await _context.Posicoes.ToListAsync();
        Assert.Single(posicoes);
        Assert.Equal(1, posicoes[0].UsuarioId);
        Assert.Equal(2, posicoes[0].AtivoId);
        Assert.Equal(100, posicoes[0].Qtd);

        _mockLogger.Verify(x => x.LogInfo(It.Is<string>(s => s.Contains("Posição adicionada"))), Times.Once);
    }

    [Fact]
    public async Task GetByUsuarioAtivoAsync_DeveRetornarPosicao_QuandoExiste()
    {
        var posicao = new Posicao
        {
            Id = 1,
            UsuarioId = 10,
            AtivoId = 20,
            Qtd = 50,
            PrecoMedio = 30m,
            Pl = 1500m
        };

        await _context.Posicoes.AddAsync(posicao);
        await _context.SaveChangesAsync();

        var resultado = await _repository.GetByUsuarioAtivoAsync(10, 20);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal(10, resultado.UsuarioId);
        Assert.Equal(20, resultado.AtivoId);
        Assert.Equal(50, resultado.Qtd);
    }

    [Fact]
    public async Task GetByUsuarioAtivoAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        var resultado = await _repository.GetByUsuarioAtivoAsync(999, 999);
        Assert.Null(resultado);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarPosicao_QuandoExiste()
    {
        var posicao = new Posicao
        {
            Id = 1,
            UsuarioId = 1,
            AtivoId = 1,
            Qtd = 10,
            PrecoMedio = 20m,
            Pl = 200m
        };

        await _context.Posicoes.AddAsync(posicao);
        await _context.SaveChangesAsync();

        // Atualizar valores
        var posicaoAtualizada = new Posicao
        {
            Id = 1,
            UsuarioId = 1,
            AtivoId = 1,
            Qtd = 15,
            PrecoMedio = 25m,
            Pl = 375m
        };

        await _repository.UpdateAsync(posicaoAtualizada);

        var posicaoDb = await _context.Posicoes.FindAsync(1);

        Assert.NotNull(posicaoDb);
        Assert.Equal(15, posicaoDb.Qtd);
        Assert.Equal(25m, posicaoDb.PrecoMedio);
        Assert.Equal(375m, posicaoDb.Pl);

        _mockLogger.Verify(x => x.LogInfo(It.Is<string>(s => s.Contains("Posição atualizada"))), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveLogarWarning_QuandoPosicaoNaoExiste()
    {
        var posicao = new Posicao
        {
            Id = 999,
            UsuarioId = 1,
            AtivoId = 1,
            Qtd = 15,
            PrecoMedio = 25m,
            Pl = 375m
        };

        await _repository.UpdateAsync(posicao);

        _mockLogger.Verify(x => x.LogWarning(It.Is<string>(s => s.Contains("não encontrada"))), Times.Once);
    }

    [Fact]
    public async Task AddAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Posicoes.AddAsync(It.IsAny<Posicao>(), default))
                   .Throws(new Exception("Erro simulado"));

        var repo = new PosicaoRepository(mockContext.Object, _mockLogger.Object);

        var posicao = new Posicao { UsuarioId = 1, AtivoId = 2, Qtd = 10, PrecoMedio = 5m, Pl = 50m };

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.AddAsync(posicao));
        Assert.Equal("Erro ao adicionar posição.", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Posicoes.FindAsync(It.IsAny<int>()))
                   .Throws(new Exception("Erro simulado"));

        var repo = new PosicaoRepository(mockContext.Object, _mockLogger.Object);

        var posicao = new Posicao { Id = 1, UsuarioId = 1, AtivoId = 2, Qtd = 10, PrecoMedio = 5m, Pl = 50m };

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.UpdateAsync(posicao));
        Assert.Equal("Erro ao atualizar posição.", ex.Message);
    }
}
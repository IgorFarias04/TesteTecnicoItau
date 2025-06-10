using Microsoft.EntityFrameworkCore;
using Moq;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories;

public class CotacaoRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger> _mockLogger;
    private readonly CotacaoRepository _repository;

    public CotacaoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _mockLogger = new Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger>();

        _repository = new CotacaoRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarCotacao()
    {
        var cotacao = new Cotacao
        {
            AtivoId = 1,
            PrecoUnit = 10.5m,
            DataHora = DateTime.Now
        };

        await _repository.AdicionarAsync(cotacao);

        var cotacoes = await _context.Cotacoes.ToListAsync();
        Assert.Single(cotacoes);
        Assert.Equal(10.5m, cotacoes[0].PrecoUnit);
    }

    [Fact]
    public async Task ObterMaisRecenteAsync_DeveRetornarMaisRecente()
    {
        var ativoId = 1;
        var cotacaoAntiga = new Cotacao
        {
            AtivoId = ativoId,
            PrecoUnit = 9.5m,
            DataHora = DateTime.Now.AddHours(-1)
        };

        var cotacaoRecente = new Cotacao
        {
            AtivoId = ativoId,
            PrecoUnit = 11.0m,
            DataHora = DateTime.Now
        };

        await _context.Cotacoes.AddRangeAsync(cotacaoAntiga, cotacaoRecente);
        await _context.SaveChangesAsync();

        var resultado = await _repository.ObterMaisRecenteAsync(ativoId);

        Assert.NotNull(resultado);
        Assert.Equal(11.0m, resultado?.PrecoUnit);
    }

    [Fact]
    public async Task ObterMaisRecenteAsync_DeveRetornarNull_SeNaoHouverCotacoes()
    {
        var resultado = await _repository.ObterMaisRecenteAsync(99);
        Assert.Null(resultado);
    }

    [Fact]
    public async Task AdicionarAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Cotacoes.AddAsync(It.IsAny<Cotacao>(), default))
                   .Throws(new Exception("Erro simulado"));

        var repo = new CotacaoRepository(mockContext.Object, _mockLogger.Object);

        var cotacao = new Cotacao { AtivoId = 1, PrecoUnit = 10, DataHora = DateTime.Now };

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.AdicionarAsync(cotacao));
        Assert.Equal("Erro ao adicionar cotação.", ex.Message);
    }
}
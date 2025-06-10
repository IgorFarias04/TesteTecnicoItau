using Microsoft.EntityFrameworkCore;
using Moq;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Repositories;

public class AtivoRepositoryTests
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<DbSet<Ativo>> _mockSet;
    private readonly Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger> _mockLogger;
    private readonly AtivoRepository _repository;

    public AtivoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        _mockContext = new Mock<AppDbContext>(options);
        _mockLogger = new Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger>();

        _repository = new AtivoRepository(context, _mockLogger.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddAtivo()
    {
        var ativo = new Ativo { Id = 1, Cod = "PETR4", Nome = "Petrobras" };

        await _repository.AddAsync(ativo);

        var result = await _repository.GetByIdAsync(ativo.Id);
        Assert.NotNull(result);
        Assert.Equal("PETR4", result?.Cod);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        var result = await _repository.GetByIdAsync(999);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllAtivos()
    {
        await _repository.AddAsync(new Ativo { Cod = "VALE3", Nome = "Vale" });
        await _repository.AddAsync(new Ativo { Cod = "ITUB4", Nome = "Itaú" });

        var ativos = await _repository.GetAllAsync();
        Assert.NotEmpty(ativos);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        var ativo = new Ativo { Cod = "ABEV3", Nome = "Ambev" };
        await _repository.AddAsync(ativo);

        ativo.Nome = "Ambev S/A";
        await _repository.UpdateAsync(ativo);

        var result = await _repository.GetByIdAsync(ativo.Id);
        Assert.Equal("Ambev S/A", result?.Nome);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        var ativo = new Ativo { Cod = "WEGE3", Nome = "Weg" };
        await _repository.AddAsync(ativo);

        await _repository.DeleteAsync(ativo.Id);

        var result = await _repository.GetByIdAsync(ativo.Id);
        Assert.Null(result);
    }
}
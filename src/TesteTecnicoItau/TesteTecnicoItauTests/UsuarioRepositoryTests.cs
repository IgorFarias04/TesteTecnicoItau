using Microsoft.EntityFrameworkCore;
using Moq;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories;

public class UsuarioRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger> _mockLogger;
    private readonly UsuarioRepository _repository;

    public UsuarioRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _mockLogger = new Mock<TesteTecnicoItau.Repositories.Interfaces.ILogger>();
        _repository = new UsuarioRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarUsuario()
    {
        var usuario = new Usuario
        {
            Id = 1,
            Nome = "Usuário Teste",
            Email = "teste@email.com"
        };

        await _repository.AddAsync(usuario);

        var usuarios = await _context.Usuarios.ToListAsync();
        Assert.Single(usuarios);
        Assert.Equal("Usuário Teste", usuarios[0].Nome);

        _mockLogger.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Usuário adicionado"))), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarUsuario_QuandoExiste()
    {
        var usuario = new Usuario { Id = 2, Nome = "João", Email = "joao@email.com" };
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        var resultado = await _repository.GetByIdAsync(2);

        Assert.NotNull(resultado);
        Assert.Equal("João", resultado.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        var resultado = await _repository.GetByIdAsync(999);
        Assert.Null(resultado);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosUsuarios()
    {
        await _context.Usuarios.AddRangeAsync(
            new Usuario { Id = 1, Nome = "Ana", Email = "ana@email.com" },
            new Usuario { Id = 2, Nome = "Carlos", Email = "carlos@email.com" });
        await _context.SaveChangesAsync();

        var resultado = await _repository.GetAllAsync();

        Assert.NotNull(resultado);
        Assert.Equal(2, ((List<Usuario>)resultado).Count);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarUsuario()
    {
        var usuario = new Usuario { Id = 1, Nome = "Original", Email = "orig@email.com" };
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        usuario.Nome = "Atualizado";
        usuario.Email = "atualizado@email.com";

        await _repository.UpdateAsync(usuario);

        var usuarioDb = await _context.Usuarios.FindAsync(1);
        Assert.Equal("Atualizado", usuarioDb.Nome);
        Assert.Equal("atualizado@email.com", usuarioDb.Email);

        _mockLogger.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Usuário atualizado"))), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverUsuario_QuandoExiste()
    {
        var usuario = new Usuario { Id = 10, Nome = "Para Deletar", Email = "delete@email.com" };
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(10);

        var usuarioDb = await _context.Usuarios.FindAsync(10);
        Assert.Null(usuarioDb);

        _mockLogger.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Usuário deletado"))), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveLogarWarning_QuandoUsuarioNaoExiste()
    {
        await _repository.DeleteAsync(999);

        _mockLogger.Verify(l => l.LogWarning(It.Is<string>(s => s.Contains("não encontrado"))), Times.Once);
    }

    [Fact]
    public async Task AddAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Usuarios.AddAsync(It.IsAny<Usuario>(), default))
                   .Throws(new Exception("Erro simulado"));

        var repo = new UsuarioRepository(mockContext.Object, _mockLogger.Object);

        var usuario = new Usuario { Id = 1, Nome = "Teste", Email = "teste@email.com" };

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.AddAsync(usuario));
        Assert.Equal("Erro ao adicionar usuário.", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Usuarios.Update(It.IsAny<Usuario>()))
                   .Throws(new Exception("Erro simulado"));

        var repo = new UsuarioRepository(mockContext.Object, _mockLogger.Object);
        var usuario = new Usuario { Id = 1, Nome = "Teste", Email = "teste@email.com" };

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.UpdateAsync(usuario));
        Assert.Equal("Erro ao atualizar usuário.", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Usuarios.FindAsync(It.IsAny<int>()))
                   .Throws(new Exception("Erro simulado"));

        var repo = new UsuarioRepository(mockContext.Object, _mockLogger.Object);

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.DeleteAsync(1));
        Assert.Equal("Erro ao deletar usuário.", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Usuarios.FindAsync(It.IsAny<int>()))
                   .Throws(new Exception("Erro simulado"));

        var repo = new UsuarioRepository(mockContext.Object, _mockLogger.Object);

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.GetByIdAsync(1));
        Assert.Equal("Erro ao buscar usuário pelo Id 1.", ex.Message);
    }

    [Fact]
    public async Task GetAllAsync_DeveLancarRepositoryException_EmErro()
    {
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(c => c.Usuarios.ToListAsync(default))
                   .Throws(new Exception("Erro simulado"));

        var repo = new UsuarioRepository(mockContext.Object, _mockLogger.Object);

        var ex = await Assert.ThrowsAsync<RepositoryException>(() => repo.GetAllAsync());
        Assert.Equal("Erro ao buscar todos os usuários.", ex.Message);
    }
}
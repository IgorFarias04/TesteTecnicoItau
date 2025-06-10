using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories.Interfaces;

namespace TesteTecnicoItau.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public UsuarioRepository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Usuarios.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar usuário pelo Id {id}: {ex.Message}", ex);
                throw new RepositoryException($"Erro ao buscar usuário pelo Id {id}.", ex);
            }
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            try
            {
                return await _context.Usuarios.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar todos os usuários: {ex.Message}", ex);
                throw new RepositoryException("Erro ao buscar todos os usuários.", ex);
            }
        }

        public async Task AddAsync(Usuario usuario)
        {
            try
            {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                _logger.LogInfo($"Usuário adicionado: Id={usuario.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar usuário: {ex.Message}", ex);
                throw new RepositoryException("Erro ao adicionar usuário.", ex);
            }
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            try
            {
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                _logger.LogInfo($"Usuário atualizado: Id={usuario.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar usuário Id={usuario.Id}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao atualizar usuário.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario != null)
                {
                    _context.Usuarios.Remove(usuario);
                    await _context.SaveChangesAsync();
                    _logger.LogInfo($"Usuário deletado: Id={id}");
                }
                else
                {
                    _logger.LogWarning($"Usuário não encontrado para exclusão: Id={id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar usuário Id={id}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao deletar usuário.", ex);
            }
        }
    }
}

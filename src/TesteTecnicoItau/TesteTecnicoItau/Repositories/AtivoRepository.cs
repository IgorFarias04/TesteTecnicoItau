using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories.Interfaces;

namespace TesteTecnicoItau.Repositories
{
    public class AtivoRepository : IAtivoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public AtivoRepository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Ativo?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Ativos.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao buscar ativo por ID.", ex);
                throw new RepositoryException("Erro ao buscar ativo por ID.", ex);
            }
        }

        public async Task<IEnumerable<Ativo>> GetAllAsync()
        {
            try
            {
                return await _context.Ativos.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao listar ativos.", ex);
                throw new RepositoryException("Erro ao listar ativos.", ex);
            }
        }

        public async Task AddAsync(Ativo ativo)
        {
            try
            {
                await _context.Ativos.AddAsync(ativo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Erro ao adicionar ativo.", ex);
                throw new RepositoryException("Erro ao adicionar ativo.", ex);
            }
        }

        public async Task UpdateAsync(Ativo ativo)
        {
            try
            {
                _context.Ativos.Update(ativo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Erro ao atualizar ativo.", ex);
                throw new RepositoryException("Erro ao atualizar ativo.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var ativo = await _context.Ativos.FindAsync(id);
                if (ativo != null)
                {
                    _context.Ativos.Remove(ativo);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Erro ao deletar ativo.", ex);
                throw new RepositoryException("Erro ao deletar ativo.", ex);
            }
        }

        public async Task RemoveAsync(Ativo ativo)
        {
            try
            {
                _context.Ativos.Remove(ativo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Erro ao remover ativo.", ex);
                throw new RepositoryException("Erro ao remover ativo.", ex);
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories.Interfaces;

namespace TesteTecnicoItau.Repositories
{
    public class OperacaoRepository : IOperacaoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public OperacaoRepository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Operacao?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Operacoes.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar operação por id {id}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao buscar operação por id.", ex);
            }
        }

        public async Task<IEnumerable<Operacao>> GetAllAsync()
        {
            try
            {
                return await _context.Operacoes
                    .Include(o => o.Usuario)
                    .Include(o => o.Ativo)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar todas as operações: {ex.Message}", ex);
                throw new RepositoryException("Erro ao buscar todas as operações.", ex);
            }
        }

        public async Task<IEnumerable<Operacao>> GetByUsuarioAtivoAsync(int usuarioId, int ativoId)
        {
            try
            {
                return await _context.Operacoes
                    .Include(o => o.Usuario)
                    .Include(o => o.Ativo)
                    .Where(o => o.UsuarioId == usuarioId && o.AtivoId == ativoId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar operações para usuário {usuarioId} e ativo {ativoId}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao buscar operações por usuário e ativo.", ex);
            }
        }

        public async Task<IEnumerable<Operacao>> GetRecentesPorUsuarioAtivoAsync(int usuarioId, int ativoId, DateTime dataLimite)
        {
            try
            {
                return await _context.Operacoes
                    .Where(o => o.UsuarioId == usuarioId && o.AtivoId == ativoId && o.DataHora >= dataLimite)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar operações recentes para usuário {usuarioId} e ativo {ativoId} a partir de {dataLimite}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao buscar operações recentes por usuário e ativo.", ex);
            }
        }

        public async Task AddAsync(Operacao operacao)
        {
            try
            {
                await _context.Operacoes.AddAsync(operacao);
                await _context.SaveChangesAsync();
                _logger.LogInfo($"Operação adicionada para usuário {operacao.UsuarioId} e ativo {operacao.AtivoId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar operação para usuário {operacao.UsuarioId} e ativo {operacao.AtivoId}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao adicionar operação.", ex);
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories.Interfaces;

namespace TesteTecnicoItau.Repositories
{
    public class PosicaoRepository : IPosicaoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public PosicaoRepository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Posicao?> GetByUsuarioAtivoAsync(int usuarioId, int ativoId)
        {
            try
            {
                return await _context.Posicoes
                    .AsNoTracking()
                    .Where(p => p.UsuarioId == usuarioId && p.AtivoId == ativoId)
                    .Select(p => new Posicao
                    {
                        Id = p.Id,
                        UsuarioId = p.UsuarioId,
                        AtivoId = p.AtivoId,
                        Qtd = p.Qtd,
                        PrecoMedio = p.PrecoMedio,
                        Pl = p.Pl
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar posição para usuário {usuarioId} e ativo {ativoId}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao buscar posição por usuário e ativo.", ex);
            }
        }

        public async Task AddAsync(Posicao posicao)
        {
            try
            {
                await _context.Posicoes.AddAsync(posicao);
                await _context.SaveChangesAsync();
                _logger.LogInfo($"Posição adicionada para usuário {posicao.UsuarioId} e ativo {posicao.AtivoId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar posição para usuário {posicao.UsuarioId} e ativo {posicao.AtivoId}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao adicionar posição.", ex);
            }
        }

        public async Task UpdateAsync(Posicao posicao)
        {
            try
            {
                var local = await _context.Posicoes.FindAsync(posicao.Id);
                if (local != null)
                {
                    local.Qtd = posicao.Qtd;
                    local.PrecoMedio = posicao.PrecoMedio;
                    local.Pl = posicao.Pl;

                    await _context.SaveChangesAsync();
                    _logger.LogInfo($"Posição atualizada para usuário {posicao.UsuarioId} e ativo {posicao.AtivoId}.");
                }
                else
                {
                    _logger.LogWarning($"Posição com ID {posicao.Id} não encontrada para atualização.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar posição com ID {posicao.Id}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao atualizar posição.", ex);
            }
        }
    }
}
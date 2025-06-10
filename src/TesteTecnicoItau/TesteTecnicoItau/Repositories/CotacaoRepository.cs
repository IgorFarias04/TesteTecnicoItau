using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Exceptions;
using TesteTecnicoItau.Repositories.Interfaces;

namespace TesteTecnicoItau.Repositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public CotacaoRepository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cotacao?> ObterMaisRecenteAsync(int ativoId)
        {
            try
            {
                return await _context.Cotacoes
                    .AsNoTracking()
                    .Where(c => c.AtivoId == ativoId)
                    .OrderByDescending(c => c.DataHora)
                    .Select(c => new Cotacao
                    {
                        Id = c.Id,
                        AtivoId = c.AtivoId,
                        PrecoUnit = c.PrecoUnit,
                        DataHora = c.DataHora
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter cotação mais recente para ativoId {ativoId}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao obter cotação mais recente.", ex);
            }
        }

        public async Task AdicionarAsync(Cotacao cotacao)
        {
            try
            {
                await _context.Cotacoes.AddAsync(cotacao);
                await _context.SaveChangesAsync();
                _logger.LogInfo($"Cotação adicionada com sucesso para ativoId {cotacao.AtivoId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar cotação para ativoId {cotacao.AtivoId}: {ex.Message}", ex);
                throw new RepositoryException("Erro ao adicionar cotação.", ex);
            }
        }
    }
}
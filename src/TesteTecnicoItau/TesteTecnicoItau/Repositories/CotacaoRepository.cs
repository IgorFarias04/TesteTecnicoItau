using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;

public class CotacaoRepository : ICotacaoRepository
{
    private readonly AppDbContext _context;

    public CotacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cotacao?> ObterMaisRecenteAsync(int ativoId)
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

    public async Task AdicionarAsync(Cotacao cotacao)
    {
        await _context.Cotacoes.AddAsync(cotacao);
        await _context.SaveChangesAsync();
    }
}


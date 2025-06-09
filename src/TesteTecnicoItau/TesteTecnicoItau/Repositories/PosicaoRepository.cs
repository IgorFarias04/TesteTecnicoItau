using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;

public class PosicaoRepository : IPosicaoRepository
{
    private readonly AppDbContext _context;

    public PosicaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Posicao?> GetByUsuarioAtivoAsync(int usuarioId, int ativoId)
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


    public async Task AddAsync(Posicao posicao)
    {
        await _context.Posicoes.AddAsync(posicao);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Posicao posicao)
    {
        var local = await _context.Posicoes.FindAsync(posicao.Id);
        if (local != null)
        {
            // Atualiza manualmente os campos
            local.Qtd = posicao.Qtd;
            local.PrecoMedio = posicao.PrecoMedio;
            local.Pl = posicao.Pl;

            await _context.SaveChangesAsync();
        }
    }

}


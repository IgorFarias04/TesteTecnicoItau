using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;

public class AtivoRepository : IAtivoRepository
{
    private readonly AppDbContext _context;

    public AtivoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Ativo?> GetByIdAsync(int id)
    {
        return await _context.Ativos.FindAsync(id);
    }

    public async Task<IEnumerable<Ativo>> GetAllAsync()
    {
        return await _context.Ativos.ToListAsync();
    }

    public async Task AddAsync(Ativo ativo)
    {
        await _context.Ativos.AddAsync(ativo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ativo ativo)
    {
        _context.Ativos.Update(ativo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var ativo = await _context.Ativos.FindAsync(id);
        if (ativo != null)
        {
            _context.Ativos.Remove(ativo);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(Ativo ativo)
    {
        _context.Ativos.Remove(ativo);
        await _context.SaveChangesAsync();
    }
}

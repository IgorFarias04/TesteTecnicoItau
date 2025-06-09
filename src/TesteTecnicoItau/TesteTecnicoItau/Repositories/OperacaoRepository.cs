using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;

public class OperacaoRepository : IOperacaoRepository
{
    private readonly AppDbContext _context;

    public OperacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Operacao?> GetByIdAsync(int id)
    {
        return await _context.Operacoes.FindAsync(id);
    }

    public async Task<IEnumerable<Operacao>> GetAllAsync()
    {
        return await _context.Operacoes
            .Include(o => o.Usuario)
            .Include(o => o.Ativo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Operacao>> GetByUsuarioAtivoAsync(int usuarioId, int ativoId)
    {
        return await _context.Operacoes
            .Include(o => o.Usuario)
            .Include(o => o.Ativo)
            .Where(o => o.UsuarioId == usuarioId && o.AtivoId == ativoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Operacao>> GetRecentesPorUsuarioAtivoAsync(int usuarioId, int ativoId, DateTime dataLimite)
    {
        return await _context.Operacoes
            .Where(o => o.UsuarioId == usuarioId && o.AtivoId == ativoId && o.DataHora >= dataLimite)
            .ToListAsync();
    }

    public async Task AddAsync(Operacao operacao)
    {
        await _context.Operacoes.AddAsync(operacao);
        await _context.SaveChangesAsync();
    }
}

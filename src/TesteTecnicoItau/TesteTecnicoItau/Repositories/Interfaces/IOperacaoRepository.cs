using TesteTecnicoItau.Entities;

public interface IOperacaoRepository
{
    Task<Operacao?> GetByIdAsync(int id);
    Task<IEnumerable<Operacao>> GetAllAsync();
    Task<IEnumerable<Operacao>> GetByUsuarioAtivoAsync(int usuarioId, int ativoId);
    Task<IEnumerable<Operacao>> GetRecentesPorUsuarioAtivoAsync(int usuarioId, int ativoId, DateTime dataLimite);
    Task AddAsync(Operacao operacao);
}

using TesteTecnicoItau.Entities;

public interface IAtivoRepository
{
    Task<Ativo?> GetByIdAsync(int id);
    Task<IEnumerable<Ativo>> GetAllAsync();
    Task AddAsync(Ativo ativo);
    Task UpdateAsync(Ativo ativo);
    Task DeleteAsync(int id);
    Task RemoveAsync(Ativo ativo);
}

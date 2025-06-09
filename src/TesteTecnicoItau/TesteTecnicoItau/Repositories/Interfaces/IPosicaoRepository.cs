using TesteTecnicoItau.Entities;

public interface IPosicaoRepository
{
    Task<Posicao?> GetByUsuarioAtivoAsync(int usuarioId, int ativoId);
    Task AddAsync(Posicao posicao);
    Task UpdateAsync(Posicao posicao);
}

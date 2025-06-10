public interface ICotacaoRepository
{
    Task<Cotacao?> ObterMaisRecenteAsync(int ativoId);
    Task AdicionarAsync(Cotacao cotacao);
}
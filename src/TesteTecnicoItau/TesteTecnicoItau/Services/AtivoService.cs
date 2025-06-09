using TesteTecnicoItau.Entities;

public class AtivoService
{
    private readonly IAtivoRepository _ativoRepository;
    private readonly ICotacaoRepository _cotacaoRepository;

    public AtivoService(IAtivoRepository ativoRepository, ICotacaoRepository cotacaoRepository)
    {
        _ativoRepository = ativoRepository;
        _cotacaoRepository = cotacaoRepository;
    }

    public async Task CadastrarAtivoAsync(string cod, string nome, decimal precoAtual)
    {
        var ativo = new Ativo
        {
            Cod = cod,
            Nome = nome
        };

        await _ativoRepository.AddAsync(ativo);

        var cotacao = new Cotacao
        {
            AtivoId = ativo.Id,
            PrecoUnit = precoAtual,
            DataHora = DateTime.Now
        };

        await _cotacaoRepository.AdicionarAsync(cotacao);

        Console.WriteLine("Ativo e cotação cadastrados com sucesso!");
    }

    public async Task<IEnumerable<Ativo>> ListarTodosAsync()
    {
        return await _ativoRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Ativo>> ListarAtivosAsync()
    {
        return await _ativoRepository.GetAllAsync();
    }

    public async Task<Ativo?> BuscarPorIdAsync(int id)
    {
        return await _ativoRepository.GetByIdAsync(id);
    }

    public async Task<Ativo?> BuscarAtivoPorIdAsync(int id)
    {
        return await _ativoRepository.GetByIdAsync(id);
    }

    public async Task AtualizarAtivoAsync(Ativo ativo)
    {
        await _ativoRepository.UpdateAsync(ativo);
        Console.WriteLine("Ativo atualizado com sucesso.");
    }

    public async Task RemoverAtivoAsync(int id)
    {
        var ativo = await _ativoRepository.GetByIdAsync(id);
        if (ativo != null)
        {
            await _ativoRepository.RemoveAsync(ativo);
            Console.WriteLine("Ativo removido com sucesso.");
        }
        else
        {
            Console.WriteLine("Ativo não encontrado.");
        }
    }
}

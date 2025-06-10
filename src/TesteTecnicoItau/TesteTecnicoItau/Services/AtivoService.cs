using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Repositories.Interfaces;

public class AtivoService
{
    private readonly IAtivoRepository _ativoRepository;
    private readonly ICotacaoRepository _cotacaoRepository;
    private readonly ILogger _logger;

    public AtivoService(IAtivoRepository ativoRepository, ICotacaoRepository cotacaoRepository, ILogger logger)
    {
        _ativoRepository = ativoRepository;
        _cotacaoRepository = cotacaoRepository;
        _logger = logger;
    }

    public async Task CadastrarAtivoAsync(string cod, string nome, decimal precoAtual)
    {
        try
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

            _logger.LogInfo($"Ativo '{cod}' cadastrado com cotação de {precoAtual:C}.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao cadastrar ativo '{cod}': {ex.Message}", ex);
            throw;
        }
    }

    public async Task<IEnumerable<Ativo>> ListarTodosAsync()
    {
        try
        {
            return await _ativoRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao listar ativos: {ex.Message}", ex);
            throw;
        }
    }

    public async Task<Ativo?> BuscarAtivoPorIdAsync(int id)
    {
        try
        {
            return await _ativoRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao buscar ativo por Id={id}: {ex.Message}", ex);
            throw;
        }
    }

    public async Task AtualizarAtivoAsync(Ativo ativo)
    {
        try
        {
            await _ativoRepository.UpdateAsync(ativo);
            _logger.LogInfo($"Ativo atualizado: Id={ativo.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao atualizar ativo Id={ativo.Id}: {ex.Message}", ex);
            throw;
        }
    }

    public async Task RemoverAtivoAsync(int id)
    {
        try
        {
            var ativo = await _ativoRepository.GetByIdAsync(id);
            if (ativo != null)
            {
                await _ativoRepository.RemoveAsync(ativo);
                _logger.LogInfo($"Ativo removido: Id={id}");
            }
            else
            {
                _logger.LogWarning($"Tentativa de remover ativo inexistente: Id={id}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao remover ativo Id={id}: {ex.Message}", ex);
            throw;
        }
    }
}
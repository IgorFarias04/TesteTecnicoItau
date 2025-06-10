using Microsoft.Extensions.Logging;
using TesteTecnicoItau.Entities;

public class OperacaoService
{
    private readonly IOperacaoRepository _operacaoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAtivoRepository _ativoRepository;
    private readonly PosicaoService _posicaoService;
    private readonly ILogger<OperacaoService> _logger;

    public OperacaoService(
        IOperacaoRepository operacaoRepo,
        IUsuarioRepository usuarioRepo,
        IAtivoRepository ativoRepo,
        PosicaoService posicaoService,
        ILogger<OperacaoService> logger)
    {
        _operacaoRepository = operacaoRepo;
        _usuarioRepository = usuarioRepo;
        _ativoRepository = ativoRepo;
        _posicaoService = posicaoService;
        _logger = logger;
    }

    public async Task RealizarOperacaoAsync(int usuarioId, int ativoId, int qtd, decimal precoUnit, string tipoOp)
    {
        try
        {
            if (qtd <= 0 || precoUnit <= 0 || string.IsNullOrWhiteSpace(tipoOp))
            {
                _logger.LogWarning("Operação inválida: qtd={Qtd}, preco={Preco}, tipoOp='{TipoOp}'", qtd, precoUnit, tipoOp);
                throw new ArgumentException("Dados inválidos para a operação.");
            }

            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId)
                          ?? throw new Exception("Usuário não encontrado.");

            var ativo = await _ativoRepository.GetByIdAsync(ativoId)
                         ?? throw new Exception("Ativo não encontrado.");

            var valorTotal = qtd * precoUnit;
            var valorCorretagem = valorTotal * (usuario.PercCorretagem / 100);

            var operacao = new Operacao
            {
                UsuarioId = usuarioId,
                AtivoId = ativoId,
                Qtd = qtd,
                PrecoUnit = precoUnit,
                TipoOp = tipoOp.ToUpper(),
                Corretagem = valorCorretagem,
                DataHora = DateTime.Now
            };

            await _operacaoRepository.AddAsync(operacao);
            await _posicaoService.AtualizarPosicaoAsync(operacao);

            _logger.LogInformation("Operação realizada: UsuarioId={UsuarioId}, AtivoId={AtivoId}, Tipo={Tipo}, Qtd={Qtd}, Preco={PrecoUnit}",
                usuarioId, ativoId, tipoOp, qtd, precoUnit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar operação para UsuarioId={UsuarioId}, AtivoId={AtivoId}", usuarioId, ativoId);
            throw;
        }
    }

    public async Task<IEnumerable<Operacao>> ListarOperacoesAsync()
    {
        try
        {
            return await _operacaoRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar operações");
            throw;
        }
    }

    public async Task<Operacao?> BuscarPorIdAsync(int id)
    {
        try
        {
            return await _operacaoRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar operação por Id={Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Operacao>> BuscarPorUsuarioEAtivoAsync(int usuarioId, int ativoId)
    {
        try
        {
            return await _operacaoRepository.GetByUsuarioAtivoAsync(usuarioId, ativoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar operações por UsuarioId={UsuarioId} e AtivoId={AtivoId}", usuarioId, ativoId);
            throw;
        }
    }
}
using TesteTecnicoItau.Entities;

public class OperacaoService
{
    private readonly IOperacaoRepository _operacaoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAtivoRepository _ativoRepository;
    private readonly PosicaoService _posicaoService;

    public OperacaoService(
        IOperacaoRepository operacaoRepo,
        IUsuarioRepository usuarioRepo,
        IAtivoRepository ativoRepo,
        PosicaoService posicaoService)
    {
        _operacaoRepository = operacaoRepo;
        _usuarioRepository = usuarioRepo;
        _ativoRepository = ativoRepo;
        _posicaoService = posicaoService;
    }

    public async Task RealizarOperacaoAsync(int usuarioId, int ativoId, int qtd, decimal precoUnit, string tipoOp)
    {
        if (qtd <= 0 || precoUnit <= 0 || string.IsNullOrWhiteSpace(tipoOp))
            throw new ArgumentException("Dados inválidos para a operação.");

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
    }

    public async Task<IEnumerable<Operacao>> ListarOperacoesAsync()
    {
        return await _operacaoRepository.GetAllAsync();
    }

    public async Task<Operacao?> BuscarPorIdAsync(int id)
    {
        return await _operacaoRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Operacao>> BuscarPorUsuarioEAtivoAsync(int usuarioId, int ativoId)
    {
        return await _operacaoRepository.GetByUsuarioAtivoAsync(usuarioId, ativoId);
    }
}


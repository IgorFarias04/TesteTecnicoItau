using Microsoft.Extensions.Logging;
using TesteTecnicoItau.Entities;

public class PosicaoService
{
    private readonly IPosicaoRepository _posicaoRepository;
    private readonly ICotacaoRepository _cotacaoRepository;
    private readonly ILogger<PosicaoService> _logger;

    public PosicaoService(IPosicaoRepository posicaoRepository, ICotacaoRepository cotacaoRepository, ILogger<PosicaoService> logger)
    {
        _posicaoRepository = posicaoRepository;
        _cotacaoRepository = cotacaoRepository;
        _logger = logger;
    }

    public async Task AtualizarPosicaoAsync(Operacao operacao)
    {
        try
        {
            var posicao = await _posicaoRepository.GetByUsuarioAtivoAsync(operacao.UsuarioId, operacao.AtivoId);

            if (operacao.TipoOp == "COMPRA")
            {
                if (posicao == null)
                {
                    posicao = new Posicao
                    {
                        UsuarioId = operacao.UsuarioId,
                        AtivoId = operacao.AtivoId,
                        Qtd = operacao.Qtd,
                        PrecoMedio = operacao.PrecoUnit,
                        Pl = 0
                    };

                    _logger.LogInformation("[Nova Posição] UsuarioId={UsuarioId}, AtivoId={AtivoId}, Qtd={Qtd}, PM={PM}",
                        posicao.UsuarioId, posicao.AtivoId, posicao.Qtd, posicao.PrecoMedio);

                    await _posicaoRepository.AddAsync(posicao);
                }
                else
                {
                    int qtdTotal = posicao.Qtd + operacao.Qtd;
                    decimal valorTotal = (posicao.Qtd * posicao.PrecoMedio) + (operacao.Qtd * operacao.PrecoUnit);

                    posicao.Qtd = qtdTotal;
                    posicao.PrecoMedio = valorTotal / qtdTotal;

                    _logger.LogInformation("[Compra Atualizada] UsuarioId={UsuarioId}, AtivoId={AtivoId}, NovaQtd={Qtd}, NovoPM={PM}",
                        posicao.UsuarioId, posicao.AtivoId, posicao.Qtd, posicao.PrecoMedio);

                    await _posicaoRepository.UpdateAsync(posicao);
                }
            }
            else if (operacao.TipoOp == "VENDA")
            {
                if (posicao == null || posicao.Qtd < operacao.Qtd)
                {
                    _logger.LogWarning("[Erro Venda] Quantidade insuficiente. UsuarioId={UsuarioId}, AtivoId={AtivoId}, QtdOperacao={Qtd}",
                        operacao.UsuarioId, operacao.AtivoId, operacao.Qtd);
                    throw new Exception("Quantidade insuficiente para venda.");
                }

                posicao.Qtd -= operacao.Qtd;

                _logger.LogInformation("[Venda] UsuarioId={UsuarioId}, AtivoId={AtivoId}, QtdRestante={Qtd}",
                    posicao.UsuarioId, posicao.AtivoId, posicao.Qtd);

                if (posicao.Qtd == 0)
                {
                    posicao.PrecoMedio = 0;
                    posicao.Pl = 0;
                    _logger.LogInformation("[Zerou Posição] UsuarioId={UsuarioId}, AtivoId={AtivoId}", posicao.UsuarioId, posicao.AtivoId);
                }

                await _posicaoRepository.UpdateAsync(posicao);
            }

            // Recarrega para garantir estado mais recente
            posicao = await _posicaoRepository.GetByUsuarioAtivoAsync(operacao.UsuarioId, operacao.AtivoId);

            // Atualiza P&L com base na cotação mais recente
            var cotacaoAtual = await _cotacaoRepository.ObterMaisRecenteAsync(operacao.AtivoId);
            if (cotacaoAtual != null && posicao != null && posicao.Qtd > 0)
            {
                posicao.Pl = (cotacaoAtual.PrecoUnit - posicao.PrecoMedio) * posicao.Qtd;
                _logger.LogInformation("[Atualização P&L] UsuarioId={UsuarioId}, AtivoId={AtivoId}, Cotacao={Cotacao}, PM={PM}, Qtd={Qtd}, PL={PL}",
                    posicao.UsuarioId, posicao.AtivoId, cotacaoAtual.PrecoUnit, posicao.PrecoMedio, posicao.Qtd, posicao.Pl);

                await _posicaoRepository.UpdateAsync(posicao);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar posição para UsuarioId={UsuarioId}, AtivoId={AtivoId}", operacao.UsuarioId, operacao.AtivoId);
            throw;
        }
    }

    public async Task<Posicao?> ObterPosicaoAsync(int usuarioId, int ativoId)
    {
        try
        {
            return await _posicaoRepository.GetByUsuarioAtivoAsync(usuarioId, ativoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter posição para UsuarioId={UsuarioId}, AtivoId={AtivoId}", usuarioId, ativoId);
            throw;
        }
    }
}
using TesteTecnicoItau.Entities;

public class PosicaoService
{
    private readonly IPosicaoRepository _posicaoRepository;
    private readonly ICotacaoRepository _cotacaoRepository;

    public PosicaoService(IPosicaoRepository posicaoRepository, ICotacaoRepository cotacaoRepository)
    {
        _posicaoRepository = posicaoRepository;
        _cotacaoRepository = cotacaoRepository;
    }

    public async Task AtualizarPosicaoAsync(Operacao operacao)
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

                Console.WriteLine($"[Nova Posição] Qtd: {posicao.Qtd}, PM: {posicao.PrecoMedio}");
                await _posicaoRepository.AddAsync(posicao);
            }
            else
            {
                int qtdTotal = posicao.Qtd + operacao.Qtd;
                decimal valorTotal = (posicao.Qtd * posicao.PrecoMedio) + (operacao.Qtd * operacao.PrecoUnit);

                posicao.Qtd = qtdTotal;
                posicao.PrecoMedio = valorTotal / qtdTotal;

                Console.WriteLine($"[Compra Atualizada] Qtd: {posicao.Qtd}, Novo PM: {posicao.PrecoMedio}");
                await _posicaoRepository.UpdateAsync(posicao);
            }
        }
        else if (operacao.TipoOp == "VENDA")
        {
            if (posicao == null || posicao.Qtd < operacao.Qtd)
            {
                Console.WriteLine("[Erro] Quantidade insuficiente para venda.");
                throw new Exception("Quantidade insuficiente para venda.");
            }

            posicao.Qtd -= operacao.Qtd;
            Console.WriteLine($"[Venda] Qtd restante: {posicao.Qtd}");

            if (posicao.Qtd == 0)
            {
                posicao.PrecoMedio = 0;
                posicao.Pl = 0;
                Console.WriteLine("[Zerou posição] PM e P&L zerados");
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
            Console.WriteLine($"[P&L] Cotação: {cotacaoAtual.PrecoUnit}, PM: {posicao.PrecoMedio}, Qtd: {posicao.Qtd}, PL: {posicao.Pl}");
            await _posicaoRepository.UpdateAsync(posicao);
        }
    }

    public async Task<Posicao?> ObterPosicaoAsync(int usuarioId, int ativoId)
    {
        return await _posicaoRepository.GetByUsuarioAtivoAsync(usuarioId, ativoId);
    }

}

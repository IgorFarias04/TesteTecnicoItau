namespace TesteTecnicoItau.Services.Menus
{
    public class PosicaoMenu
    {
        private readonly PosicaoService _posicaoService;

        public PosicaoMenu(PosicaoService posicaoService)
        {
            _posicaoService = posicaoService;
        }

        public async Task ExibirMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("--- CONSULTAR POSIÇÃO ---");

            Console.Write("ID do usuário: ");
            var inputUsuario = Console.ReadLine();
            if (!int.TryParse(inputUsuario, out int idUsuario))
            {
                Console.WriteLine("ID do usuário inválido.");
                return;
            }

            Console.Write("ID do ativo: ");
            var inputAtivo = Console.ReadLine();
            if (!int.TryParse(inputAtivo, out int idAtivo))
            {
                Console.WriteLine("ID do ativo inválido.");
                return;
            }

            var posicao = await _posicaoService.ObterPosicaoAsync(idUsuario, idAtivo);

            Console.WriteLine();
            Console.WriteLine("Resumo da Posição:");

            if (posicao != null)
            {
                Console.WriteLine($"Ativo: {posicao.AtivoId} - Quantidade: {posicao.Quantidade} - Preço Médio: {posicao.PrecoMedio:F2}");
            }
            else
            {
                Console.WriteLine("Nenhuma posição encontrada para os dados informados.");
            }

            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}
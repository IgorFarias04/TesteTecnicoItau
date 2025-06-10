namespace TesteTecnicoItau.Services.Menus
{
    public class OperacaoMenu
    {
        private readonly OperacaoService _operacaoService;

        public OperacaoMenu(OperacaoService operacaoService)
        {
            _operacaoService = operacaoService;
        }

        public object TipoOperacao { get; private set; }

        public async Task ExibirMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- MENU OPERAÇÕES ---");
                Console.WriteLine("1. Cadastrar operação");
                Console.WriteLine("2. Listar operações");
                Console.WriteLine("0. Voltar");
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        Console.Write("ID do usuário: ");
                        var idUsuario = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("ID do ativo: ");
                        var idAtivo = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Tipo (C para Compra, V para Venda): ");
                        var tipo = Console.ReadLine()?.ToUpper() == "C" ? "Compra" : "Venda";

                        Console.Write("Quantidade: ");
                        var quantidade = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Preço unitário: ");
                        var preco = decimal.Parse(Console.ReadLine() ?? "0");

                        await _operacaoService.RealizarOperacaoAsync(idUsuario, idAtivo, quantidade, preco, tipo);
                        Console.WriteLine("Operação registrada com sucesso!");
                        break;

                    case "2":
                        var operacoes = await _operacaoService.ListarOperacoesAsync();
                        foreach (var o in operacoes)
                            Console.WriteLine($"{o.Tipo} - {o.Usuario?.Nome} - {o.Ativo?.Codigo} - Qtde: {o.Quantidade} - Preço: {o.PrecoUnitario} - Total: {o.TotalOperacao}");
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }
        }
    }
}
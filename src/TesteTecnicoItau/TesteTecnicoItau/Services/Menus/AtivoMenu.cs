namespace TesteTecnicoItau.Services.Menus
{
    public class AtivoMenu
    {
        private readonly AtivoService _ativoService;

        public AtivoMenu(AtivoService ativoService)
        {
            _ativoService = ativoService;
        }

        public async Task ExibirMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- MENU ATIVOS ---");
                Console.WriteLine("1. Cadastrar ativo");
                Console.WriteLine("2. Listar ativos");
                Console.WriteLine("3. Buscar por ID");
                Console.WriteLine("4. Atualizar ativo");
                Console.WriteLine("5. Remover ativo");
                Console.WriteLine("0. Voltar");
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        Console.Write("Nome: ");
                        var nome = Console.ReadLine();
                        Console.Write("Código: ");
                        var codigo = Console.ReadLine();
                        Console.Write("Preço Atual: ");
                        var precoAtual = Convert.ToDecimal(Console.ReadLine());

                        await _ativoService.CadastrarAtivoAsync(nome, codigo, precoAtual);
                        Console.WriteLine("Ativo cadastrado com sucesso!");
                        break;

                    case "2":
                        var ativos = await _ativoService.ListarTodosAsync();
                        foreach (var a in ativos)
                            Console.WriteLine($"ID: {a.Id}, Nome: {a.Nome}, Código: {a.Codigo}");
                        break;

                    case "3":
                        Console.Write("ID do ativo: ");
                        var idBusca = int.Parse(Console.ReadLine() ?? "0");
                        var ativo = await _ativoService.BuscarAtivoPorIdAsync(idBusca);
                        if (ativo != null)
                            Console.WriteLine($"ID: {ativo.Id}, Nome: {ativo.Nome}, Código: {ativo.Codigo}");
                        else
                            Console.WriteLine("Ativo não encontrado.");
                        break;

                    case "4":
                        Console.Write("ID do ativo: ");
                        var idAtualiza = int.Parse(Console.ReadLine() ?? "0");
                        var ativoAtual = await _ativoService.BuscarAtivoPorIdAsync(idAtualiza);
                        if (ativoAtual != null)
                        {
                            Console.Write("Novo nome: ");
                            ativoAtual.Nome = Console.ReadLine() ?? ativoAtual.Nome;
                            Console.Write("Novo código: ");
                            ativoAtual.Codigo = Console.ReadLine() ?? ativoAtual.Codigo;

                            await _ativoService.AtualizarAtivoAsync(ativoAtual);
                            Console.WriteLine("Ativo atualizado!");
                        }
                        else
                        {
                            Console.WriteLine("Ativo não encontrado.");
                        }
                        break;

                    case "5":
                        Console.Write("ID do ativo a remover: ");
                        var idRemove = int.Parse(Console.ReadLine() ?? "0");
                        await _ativoService.RemoverAtivoAsync(idRemove);
                        Console.WriteLine("Ativo removido (se existia).");
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
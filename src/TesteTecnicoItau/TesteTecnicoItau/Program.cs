using System;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Entities;
using TesteTecnicoItau.Services;

class Program
{
    static async Task Main(string[] args)
    {
        using var dbContext = new AppDbContext();

        var usuarioRepo = new UsuarioRepository(dbContext);
        var usuarioService = new UsuarioService(usuarioRepo);

        var cotacaoRepo = new CotacaoRepository(dbContext);

        var ativoRepo = new AtivoRepository(dbContext);
        var ativoService = new AtivoService(ativoRepo, cotacaoRepo);

        var posicaoRepo = new PosicaoRepository(dbContext);
        var posicaoService = new PosicaoService(posicaoRepo, cotacaoRepo);

        var operacaoRepo = new OperacaoRepository(dbContext);
        var operacaoService = new OperacaoService(operacaoRepo, usuarioRepo, ativoRepo, posicaoService);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- MENU PRINCIPAL ---");
            Console.WriteLine("1. Gerenciar Usuários");
            Console.WriteLine("2. Gerenciar Ativos");
            Console.WriteLine("3. Gerenciar Operações");
            Console.WriteLine("4. Consultar posições"); 
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");

            var opcao = Console.ReadLine();
            Console.WriteLine();

            switch (opcao)
            {
                case "1":
                    await MenuUsuarios(usuarioService);
                    break;
                case "2":
                    await MenuAtivos(ativoService);
                    break;
                case "3":
                    await MenuOperacoes(operacaoService);
                    break;
                case "4":
                    await MenuPosicoes(posicaoService, usuarioService, ativoService);
                    break;

                case "0":
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            Console.WriteLine("\nPressione ENTER para voltar ao menu principal...");
            Console.ReadLine();
        }
    }
    static async Task MenuUsuarios(UsuarioService usuarioService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- MENU USUÁRIOS ---");
            Console.WriteLine("1. Cadastrar usuário");
            Console.WriteLine("2. Listar usuários");
            Console.WriteLine("3. Buscar por ID");
            Console.WriteLine("4. Atualizar usuário");
            Console.WriteLine("5. Remover usuário");
            Console.WriteLine("0. Voltar");
            Console.Write("Escolha uma opção: ");

            var opcao = Console.ReadLine();
            Console.WriteLine();

            switch (opcao)
            {
                case "1":
                    Console.Write("Nome: ");
                    var nome = Console.ReadLine();
                    Console.Write("Email: ");
                    var email = Console.ReadLine();
                    Console.Write("% Corretagem: ");
                    var perc = decimal.Parse(Console.ReadLine() ?? "0");

                    await usuarioService.CadastrarUsuarioAsync(new Usuario { Nome = nome!, Email = email!, PercCorretagem = perc });
                    Console.WriteLine("Usuário cadastrado com sucesso!");
                    break;

                case "2":
                    var usuarios = await usuarioService.ListarUsuariosAsync();
                    foreach (var u in usuarios)
                        Console.WriteLine($"ID: {u.Id}, Nome: {u.Nome}, Email: {u.Email}, %Corretagem: {u.PercCorretagem}");
                    break;

                case "3":
                    Console.Write("ID do usuário: ");
                    var idBusca = int.Parse(Console.ReadLine() ?? "0");
                    var usuario = await usuarioService.BuscarUsuarioPorIdAsync(idBusca);
                    if (usuario != null)
                        Console.WriteLine($"ID: {usuario.Id}, Nome: {usuario.Nome}, Email: {usuario.Email}, %Corretagem: {usuario.PercCorretagem}");
                    else
                        Console.WriteLine("Usuário não encontrado.");
                    break;

                case "4":
                    Console.Write("ID do usuário: ");
                    var idAtualiza = int.Parse(Console.ReadLine() ?? "0");
                    var usuarioAtual = await usuarioService.BuscarUsuarioPorIdAsync(idAtualiza);
                    if (usuarioAtual != null)
                    {
                        Console.Write("Novo nome: ");
                        usuarioAtual.Nome = Console.ReadLine() ?? usuarioAtual.Nome;
                        Console.Write("Novo email: ");
                        usuarioAtual.Email = Console.ReadLine() ?? usuarioAtual.Email;
                        Console.Write("Nova % corretagem: ");
                        usuarioAtual.PercCorretagem = decimal.Parse(Console.ReadLine() ?? usuarioAtual.PercCorretagem.ToString());

                        await usuarioService.AtualizarUsuarioAsync(usuarioAtual);
                        Console.WriteLine("Usuário atualizado!");
                    }
                    else
                    {
                        Console.WriteLine("Usuário não encontrado.");
                    }
                    break;

                case "5":
                    Console.Write("ID do usuário a remover: ");
                    var idRemove = int.Parse(Console.ReadLine() ?? "0");
                    await usuarioService.RemoverUsuarioAsync(idRemove);
                    Console.WriteLine("Usuário removido (se existia).\n");
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

    static async Task MenuAtivos(AtivoService ativoService)
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
                    Console.Write("Código do ativo (ex: PETR4): ");
                    var cod = Console.ReadLine();
                    Console.Write("Nome do ativo: ");
                    var nome = Console.ReadLine();

                    //Console.Write("Preço atual (cotação): ");
                    //decimal preco = decimal.Parse(Console.ReadLine() ?? "0");

                    decimal preco;

                    Console.Write("Deseja buscar a cotação automaticamente? (s/n): ");
                    var escolha = Console.ReadLine()?.ToLower();

                    if (escolha == "s")
                    {
                        var cotacaoClient = new B3CotacaoClient();
                        var cotacao = await cotacaoClient.ObterCotacaoAsync(cod);

                        if (cotacao.HasValue)
                        {
                            preco = cotacao.Value;
                            Console.WriteLine($"Cotação automática encontrada: R$ {preco}");
                        }
                        else
                        {
                            Console.WriteLine("Falha ao obter cotação automática. Digite o valor manualmente:");
                            preco = Convert.ToDecimal(Console.ReadLine());
                        }
                    }
                    else
                    {
                        Console.Write("Digite o valor da cotação: ");
                        preco = Convert.ToDecimal(Console.ReadLine());
                    }


                    await ativoService.CadastrarAtivoAsync(cod, nome, preco);
                    Console.WriteLine("Ativo cadastrado com sucesso!");
                    break;

                case "2":
                    var ativos = await ativoService.ListarAtivosAsync();
                    foreach (var a in ativos)
                        Console.WriteLine($"ID: {a.Id}, Código: {a.Cod}, Nome: {a.Nome}");
                    break;

                case "3":
                    Console.Write("ID do ativo: ");
                    var idBusca = int.Parse(Console.ReadLine() ?? "0");
                    var ativo = await ativoService.BuscarAtivoPorIdAsync(idBusca);
                    if (ativo != null)
                        Console.WriteLine($"ID: {ativo.Id}, Código: {ativo.Cod}, Nome: {ativo.Nome}");
                    else
                        Console.WriteLine("Ativo não encontrado.");
                    break;

                case "4":
                    Console.Write("ID do ativo: ");
                    var idAtualiza = int.Parse(Console.ReadLine() ?? "0");
                    var ativoAtual = await ativoService.BuscarAtivoPorIdAsync(idAtualiza);
                    if (ativoAtual != null)
                    {
                        Console.Write("Novo código: ");
                        ativoAtual.Cod = Console.ReadLine() ?? ativoAtual.Cod;
                        Console.Write("Novo nome: ");
                        ativoAtual.Nome = Console.ReadLine() ?? ativoAtual.Nome;

                        await ativoService.AtualizarAtivoAsync(ativoAtual);
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
                    await ativoService.RemoverAtivoAsync(idRemove);
                    Console.WriteLine("Ativo removido (se existia).\n");
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

    static async Task MenuOperacoes(OperacaoService operacaoService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- MENU OPERAÇÕES ---");
            Console.WriteLine("1. Registrar operação");
            Console.WriteLine("2. Listar operações");
            Console.WriteLine("3. Buscar operação por ID");
            Console.WriteLine("4. Buscar por usuário e ativo");
            Console.WriteLine("0. Voltar");
            Console.Write("Escolha uma opção: ");

            var opcao = Console.ReadLine();
            Console.WriteLine();

            switch (opcao)
            {
                case "1":
                    Console.Write("ID do usuário: ");
                    var usuarioId = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("ID do ativo: ");
                    var ativoId = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Quantidade: ");
                    var qtd = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Preço unitário: ");
                    var precoUnit = decimal.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Tipo de operação (COMPRA/VENDA): ");
                    var tipo = Console.ReadLine() ?? "";

                    await operacaoService.RealizarOperacaoAsync(usuarioId, ativoId, qtd, precoUnit, tipo);
                    Console.WriteLine("Operação registrada com sucesso!");
                    break;

                case "2":
                    var operacoes = await operacaoService.ListarOperacoesAsync();
                    foreach (var o in operacoes)
                        Console.WriteLine($"ID: {o.Id}, Tipo: {o.TipoOp}, Quantidade: {o.Qtd}, Preço: {o.PrecoUnit}, Corretagem: {o.Corretagem}, Usuário: {o.Usuario.Nome}, Ativo: {o.Ativo.Cod}, Data: {o.DataHora}");
                    break;

                case "3":
                    Console.Write("ID da operação: ");
                    var id = int.Parse(Console.ReadLine() ?? "0");
                    var op = await operacaoService.BuscarPorIdAsync(id);
                    if (op != null)
                    {
                        Console.WriteLine($"ID: {op.Id}, Tipo: {op.TipoOp}, Quantidade: {op.Qtd}, Preço: {op.PrecoUnit}, Corretagem: {op.Corretagem}, Usuário: {op.Usuario.Nome}, Ativo: {op.Ativo.Cod}");
                    }
                    else
                    {
                        Console.WriteLine("Operação não encontrada.");
                    }
                    break;
                case "4":
                    Console.Write("ID do usuário: ");
                    int usuarioIdBusca = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("ID do ativo: ");
                    int ativoIdBusca = int.Parse(Console.ReadLine() ?? "0");

                    var ops = await operacaoService.BuscarPorUsuarioEAtivoAsync(usuarioIdBusca, ativoIdBusca);

                    if (!ops.Any())
                    {
                        Console.WriteLine("Nenhuma operação encontrada.");
                    }
                    else
                    {
                        foreach (var o in ops)
                        {
                            Console.WriteLine(
                                $"ID: {o.Id}, Tipo: {o.TipoOp}, Quantidade: {o.Qtd}, Preço: {o.PrecoUnit}, " +
                                $"Corretagem: {o.Corretagem}, Usuário: {o.Usuario?.Nome}, Ativo: {o.Ativo?.Cod}, Data: {o.DataHora:dd/MM/yyyy HH:mm:ss}");
                        }
                    }
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

    static async Task MenuPosicoes(PosicaoService posicaoService, UsuarioService usuarioService, AtivoService ativoService)
    {
        Console.Clear();
        Console.WriteLine("🔎 CONSULTAR POSIÇÕES");
        Console.Write("Digite o ID do usuário: ");
        int usuarioId = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("\nPosições encontradas:\n");

        var usuario = await usuarioService.BuscarPorIdAsync(usuarioId);
        if (usuario == null)
        {
            Console.WriteLine("Usuário não encontrado.");
            return;
        }

        foreach (var ativo in await ativoService.ListarTodosAsync())
        {
            var posicao = await posicaoService.ObterPosicaoAsync(usuarioId, ativo.Id);
            if (posicao != null && posicao.Qtd > 0)
            {
                Console.WriteLine($"Ativo: {ativo.Cod} | Quantidade: {posicao.Qtd} | Preço Médio: {posicao.PrecoMedio:C} | P&L: {posicao.Pl:C}");
            }
        }
    }



}


namespace TesteTecnicoItau.Services.Menus
{
    public class UsuarioMenu
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioMenu(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task ExibirMenuAsync()
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

                        await _usuarioService.CadastrarUsuarioAsync(new Entities.Usuario
                        {
                            Nome = nome!,
                            Email = email!,
                            PercCorretagem = perc
                        });

                        Console.WriteLine("Usuário cadastrado com sucesso!");
                        break;

                    case "2":
                        var usuarios = await _usuarioService.ListarUsuariosAsync();
                        foreach (var u in usuarios)
                            Console.WriteLine($"ID: {u.Id}, Nome: {u.Nome}, Email: {u.Email}, %Corretagem: {u.PercCorretagem}");
                        break;

                    case "3":
                        Console.Write("ID do usuário: ");
                        var idBusca = int.Parse(Console.ReadLine() ?? "0");
                        var usuario = await _usuarioService.BuscarUsuarioPorIdAsync(idBusca);
                        if (usuario != null)
                            Console.WriteLine($"ID: {usuario.Id}, Nome: {usuario.Nome}, Email: {usuario.Email}, %Corretagem: {usuario.PercCorretagem}");
                        else
                            Console.WriteLine("Usuário não encontrado.");
                        break;

                    case "4":
                        Console.Write("ID do usuário: ");
                        var idAtualiza = int.Parse(Console.ReadLine() ?? "0");
                        var usuarioAtual = await _usuarioService.BuscarUsuarioPorIdAsync(idAtualiza);
                        if (usuarioAtual != null)
                        {
                            Console.Write("Novo nome: ");
                            usuarioAtual.Nome = Console.ReadLine() ?? usuarioAtual.Nome;
                            Console.Write("Novo email: ");
                            usuarioAtual.Email = Console.ReadLine() ?? usuarioAtual.Email;
                            Console.Write("Nova % corretagem: ");
                            usuarioAtual.PercCorretagem = decimal.Parse(Console.ReadLine() ?? usuarioAtual.PercCorretagem.ToString());

                            await _usuarioService.AtualizarUsuarioAsync(usuarioAtual);
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
                        await _usuarioService.RemoverUsuarioAsync(idRemove);
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
    }
}
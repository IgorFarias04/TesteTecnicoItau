using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TesteTecnicoItau.Context;
using TesteTecnicoItau.Logging;
using TesteTecnicoItau.Repositories;
using TesteTecnicoItau.Repositories.Interfaces;
using TesteTecnicoItau.Services;
using TesteTecnicoItau.Services.Menus;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices((context, services) =>
        {
            // Logger customizado singleton
            services.AddSingleton<ILogger, ConsoleLogger>();

            // DbContext EF Core
            services.AddDbContext<AppDbContext>();

            // Repositórios
            services.AddScoped<UsuarioRepository>();
            services.AddScoped<AtivoRepository>();
            services.AddScoped<OperacaoRepository>();
            services.AddScoped<PosicaoRepository>();

            // Serviços
            services.AddScoped<UsuarioService>();
            services.AddScoped<AtivoService>();
            services.AddScoped<OperacaoService>();
            services.AddScoped<PosicaoService>();

            // Menus
            services.AddScoped<UsuarioMenu>();
            services.AddScoped<AtivoMenu>();
            services.AddScoped<OperacaoMenu>();
            services.AddScoped<PosicaoMenu>();
        });

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- SISTEMA FINANCEIRO ---");
            Console.WriteLine("1. Usuários");
            Console.WriteLine("2. Ativos");
            Console.WriteLine("3. Operações");
            Console.WriteLine("4. Posição");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");

            var opcao = Console.ReadLine();
            Console.WriteLine();

            switch (opcao)
            {
                case "1":
                    await services.GetRequiredService<UsuarioMenu>().ExibirMenuAsync();
                    break;
                case "2":
                    await services.GetRequiredService<AtivoMenu>().ExibirMenuAsync();
                    break;
                case "3":
                    await services.GetRequiredService<OperacaoMenu>().ExibirMenuAsync();
                    break;
                case "4":
                    await services.GetRequiredService<PosicaoMenu>().ExibirMenuAsync();
                    break;
                case "0":
                    Console.WriteLine("Saindo...");
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
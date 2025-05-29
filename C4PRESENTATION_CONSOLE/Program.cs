using C4PRESENTATION_CONSOLE.DTOs;
using C4PRESENTATION_CONSOLE.Interfaces.IRepositories;
using C4PRESENTATION_CONSOLE.Interfaces.IServices;
using C4PRESENTATION_CONSOLE.Mappings;
using C4PRESENTATION_CONSOLE.Repositories;
using C4PRESENTATION_CONSOLE.Services;
using C4PRESENTATION_CONSOLE.SQLite;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("fcouto1331 - P2025_SQLite\n");

#region IoC

// Configurar o contêiner de serviços
var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<SQLiteContext>();
serviceCollection.AddScoped<SQLiteConfig>();
serviceCollection.AddTransient<IClienteRepository, ClienteRepository>();
serviceCollection.AddTransient<IClienteService, ClienteService>();
var serviceProvider = serviceCollection.BuildServiceProvider();

// Resolver o contêiner de serviço
var _serviceSQLite = serviceProvider.GetService<SQLiteConfig>();
var _service = serviceProvider.GetService<IClienteService>();

#endregion

#region Presentation

if (_serviceSQLite != null)
{
    try
    {
        _serviceSQLite.Iniciar();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro SQLite: {ex.Message}");
        return;
    }
}

if (_service != null)
{
    try
    {
        bool menu = true;
        while (menu)
        {
            Console.Clear();

            Console.WriteLine("1 - Listar");
            Console.WriteLine("2 - Criar");
            Console.WriteLine("3 - Alterar");
            Console.WriteLine("4 - Excluir");
            Console.WriteLine("0 - Sair");
            Console.Write("Opção: ");

            var opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    Comandos.Listar(_service);
                    break;
                case "2":
                    Comandos.Criar(_service);
                    break;
                case "3":
                    Comandos.Alterar(_service);
                    break;
                case "4":
                    Comandos.Excluir(_service);
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            Console.WriteLine("\n");
            Console.WriteLine("1 - Menu");
            Console.WriteLine("0 - Sair");
            Console.Write("Opção: ");
            menu = Console.ReadLine() == "1" ? true : false;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
        return;
    }
}

Environment.Exit(0);

#endregion

#region Application

public class Comandos {

    public static void Listar(IClienteService _service) 
    {
        Console.WriteLine("\nListar");
        List<ClienteDTO> cliente = Mapping.ToListClienteDTO(_service.Listar());
        foreach (var item in cliente)
        {
            Console.WriteLine($"Id: {item.Id}, Nome: {item.Nome}");
        }
    }

    public static void Criar(IClienteService _service)
    {
        Console.WriteLine("\nCriar");
        Console.Write("Nome: ");
        string? nome = Console.ReadLine();
        if (!String.IsNullOrEmpty(nome))
        {
            ClienteDTO cliente = new ClienteDTO { Id = 0, Nome = nome };
            _service.Criar(Mapping.ToCliente(cliente));
        }
        Listar(_service);
    }

    public static void Alterar(IClienteService _service)
    {
        Console.WriteLine("\nAlterar");
        Console.WriteLine("Id:");
        int id = Convert.ToInt32(Console.ReadLine());
        ClienteDTO cliente = Mapping.ToClienteDTO(_service.PegarPorId(id));
        Console.WriteLine($"Id: {cliente.Id}, Nome: {cliente.Nome}");
        Console.Write("Nome: ");
        string? nome = Console.ReadLine();
        if (!String.IsNullOrEmpty(nome))
        {
            ClienteDTO cliente2 = new ClienteDTO { Id = cliente.Id, Nome = nome };
            _service.Alterar(Mapping.ToCliente(cliente2), cliente.Id);
        }
        Listar(_service);
    }

    public static void Excluir(IClienteService _service) 
    {
        Console.WriteLine("\nExcluir");
        Console.WriteLine("Id:");
        int id = Convert.ToInt32(Console.ReadLine());
        _service.Excluir(id);
        Listar(_service);
    }

}

#endregion
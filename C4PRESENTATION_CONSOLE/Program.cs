using C4PRESENTATION_CONSOLE.DTOs;
using C4PRESENTATION_CONSOLE.Interfaces.IRepositories;
using C4PRESENTATION_CONSOLE.Interfaces.IServices;
using C4PRESENTATION_CONSOLE.Mappings;
using C4PRESENTATION_CONSOLE.Repositories;
using C4PRESENTATION_CONSOLE.Services;
using C4PRESENTATION_CONSOLE.SQLite;
using Microsoft.Extensions.DependencyInjection;

#region CONFIGURAÇÃO

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

#region MENU

if (_serviceSQLite != null)
{
    try
    {
        _serviceSQLite.Iniciar();
    }
    catch (Exception ex)
    {
        throw new Exception($"Erro SQLite: {ex.Message}");
    }
}

Console.WriteLine("fcouto1331 - P2025_SQLite\n");
if (_service != null)
{
    try
    {
        bool cond = true;
        while (cond)
        {
            Console.Clear();

            Console.WriteLine("1 - Listar");
            Console.WriteLine("2 - Criar");
            Console.WriteLine("3 - Alterar");
            Console.WriteLine("4 - Excluir");
            Console.WriteLine("0 - Sair");
            Console.Write("Menu: ");

            var menu = Console.ReadLine();
            switch (menu)
            {
                case "1":
                    ClienteApp.Listar(_service);
                    break;
                case "2":
                    ClienteApp.Criar(_service);
                    break;
                case "3":
                    ClienteApp.Alterar(_service);
                    break;
                case "4":
                    ClienteApp.Excluir(_service);
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    throw new Exception("Opção inválida");
            }

            Console.WriteLine("\n1 - Menu");
            Console.WriteLine("0 - Sair");
            Console.Write("SubMenu: ");
            cond = Console.ReadLine() == "1" ? true : false;
        }
    }
    catch (Exception ex)
    {
        throw new Exception($"Erro: {ex.Message}");
    }
}

Environment.Exit(0);

#endregion

#region COMANDOS

public class ClienteApp {

    public static void Listar(IClienteService _service) 
    {
        Console.WriteLine("\nListar");
        List<ClienteDTO> cliente = Mapper.ToListClienteDTO(_service.Listar());
        foreach (var item in cliente)
        {
            Console.WriteLine($"Id: {item.Id}, Nome: {item.Nome}, IdGuid: {item.IdGuid}");
        }
    }

    public static void Criar(IClienteService _service)
    {
        Console.WriteLine("\nCriar");
        Console.Write("Nome: ");
        string? nome = Console.ReadLine();
        if (!String.IsNullOrEmpty(nome))
        {
            ClienteDTO cliente = new ClienteDTO { Id = 0, Nome = nome, IdGuid = Guid.NewGuid() };
            _service.Criar(Mapper.ToCliente(cliente));
        }
        Listar(_service);
    }

    public static void Alterar(IClienteService _service)
    {
        Console.WriteLine("\nAlterar");
        Console.Write("Id:");
        int id = Convert.ToInt32(Console.ReadLine());
        ClienteDTO cliente = Mapper.ToClienteDTO(_service.PegarPorId(id));
        Console.WriteLine($"Id: {cliente.Id}, Nome: {cliente.Nome}, IdGuid: {cliente.IdGuid}");
        Console.Write("Nome: ");
        string nome = Console.ReadLine() ?? String.Empty;
        if (!String.IsNullOrEmpty(nome))
        {
            ClienteDTO cliente2 = new ClienteDTO { Id = cliente.Id, Nome = nome, IdGuid = cliente.IdGuid };
            _service.Alterar(Mapper.ToCliente(cliente2), cliente.Id);
        }
        Listar(_service);
    }

    public static void Excluir(IClienteService _service) 
    {
        Console.WriteLine("\nExcluir");
        Console.Write("Id:");
        int id = Convert.ToInt32(Console.ReadLine());
        _service.Excluir(id);
        Listar(_service);
    }
}

#endregion
using C4PRESENTATION_CONSOLE.Entities;
using C4PRESENTATION_CONSOLE.Interfaces.IRepositories;
using C4PRESENTATION_CONSOLE.SQLite;
using Microsoft.Data.Sqlite;
using System.Data;

namespace C4PRESENTATION_CONSOLE.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly SQLiteContext _context;
        public ClienteRepository(SQLiteContext context)
        {
            _context = context;
        }

        public void Alterar(Cliente cliente, int id)
        {
            using (var db = _context.CreateConnection())
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Cliente SET Nome = @Nome WHERE Id = @Id";
                    var nome = new SqliteParameter("@Nome", DbType.String) { Value = cliente.Nome };
                    var _id = new SqliteParameter("@Id", DbType.Int32) { Value = cliente.Id };
                    cmd.Parameters.Add(nome);
                    cmd.Parameters.Add(_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Criar(Cliente cliente)
        {
            using (var db = _context.CreateConnection())
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Cliente (Nome) VALUES (@Nome)";
                    var nome = new SqliteParameter("@Nome", DbType.String) { Value = cliente.Nome };
                    cmd.Parameters.Add(nome);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Excluir(int id)
        {
            using (var db = _context.CreateConnection())
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Cliente WHERE Id = @Id";
                    var _id = new SqliteParameter("@Id", DbType.Int32) { Value = id };
                    cmd.Parameters.Add(_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Cliente> Listar()
        {
            using (var db = _context.CreateConnection())
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Nome FROM Cliente";
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Cliente> lstCliente = new List<Cliente>();
                        while (reader.Read())
                        {
                            var cliente = new Cliente(reader.GetInt32(0), reader.GetString(1)) { };
                            lstCliente.Add(cliente);
                        }
                        return lstCliente;
                    }
                }
            }
        }

        public Cliente PegarPorId(int id)
        {
            using (var db = _context.CreateConnection())
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Nome FROM Cliente WHERE Id = @Id";
                    var _id = new SqliteParameter("@Id", DbType.Int32) { Value = id };
                    cmd.Parameters.Add(_id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Cliente cliente = new Cliente();
                        if (reader.Read())
                        {
                            cliente = new Cliente(reader.GetInt32(0), reader.GetString(1));
                        }
                        return cliente;
                    }
                }
            }
        }
    }
}

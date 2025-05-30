using C4PRESENTATION_CONSOLE.DTOs;
using C4PRESENTATION_CONSOLE.Entities;

namespace C4PRESENTATION_CONSOLE.Mappings
{
    public partial class Mapping
    {
        public static ClienteDTO ToClienteDTO(Cliente cliente)
        {
            var clienteDTO = new ClienteDTO
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                IdGuid = cliente.IdGuid
            };
            return clienteDTO;
        }

        public static List<ClienteDTO> ToListClienteDTO(List<Cliente> lstCliente)
        {
            return [.. lstCliente.Select(ToClienteDTO)];
        }
    }
}

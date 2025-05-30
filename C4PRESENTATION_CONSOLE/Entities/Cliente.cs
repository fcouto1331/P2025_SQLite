namespace C4PRESENTATION_CONSOLE.Entities
{
    public class Cliente
    {
        public Cliente() { }
        // método 1
        public Cliente(int id, string nome, Guid idGuid)
        {
            Id = id;
            Nome = nome.ToUpper();
            IdGuid = idGuid;
        }

        public int Id { get; private set; }
        // método 2
        public void AlterarId(int id)
        {
            Id = id;
        }

        public string? Nome { get; private set; }
        public void AlterarNome(string nome)
        {
            Nome = nome.ToUpper();
        }

        public Guid IdGuid { get; set; }
        public void AlterarIdGuid(Guid idGuid)
        {
            IdGuid = idGuid;

        }
    }
}
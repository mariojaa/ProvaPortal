namespace ProvaPortal.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataHora { get; set; }
        public string UsuarioId { get; set; }

    }
}

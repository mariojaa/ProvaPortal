using ProvaPortal.Models;

public class ProvaModel
{
    public int Id { get; set; }
    public string NomeArquivo { get; set; }
    public DateTime DataEnvio { get; set; }
    public int NumeroCopias { get; set; }
    public int IdProfessor { get; set; }
    public ProfessorModel UsuarioLogin { get; set; }
}

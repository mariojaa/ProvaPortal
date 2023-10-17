using ProvaPortal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

public class ProvaModel //Dependente //Contato
{
    public Nullable <int> Id { get; set; }
    public string NomeArquivo { get; set; }
    public DateTime DataEnvio { get; set; }
    public int NumeroCopias { get; set; }
    [ForeignKey("ProfessorModel")]
    public int ProfessorId { get; set; }
    public ProfessorModel Professor { get; set; }
}
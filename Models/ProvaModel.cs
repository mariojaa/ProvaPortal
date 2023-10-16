using ProvaPortal.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class ProvaModel //Dependente //Contato
{
    public int Id { get; set; }
    public string NomeArquivo { get; set; }
    public DateTime DataEnvio { get; set; }
    public int NumeroCopias { get; set; }

    [ForeignKey("ProfessorModel")]
    public int ProfessorId { get; set; }
    public ProfessorModel ProfessorModel { get; set; }
}
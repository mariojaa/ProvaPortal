using ProvaPortal.Models;
using ProvaPortal.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;
//8b6baafeccd4dcfe46721e1b796cde1c5b3b3d47
public class ProvaModel //Dependente //Contato
{
    public Nullable <int> Id { get; set; }
    public string NomeArquivo { get; set; }
    public DateTime DataEnvio { get; set; }
    public DateTime DataImpressao { get; set; }
    public int NumeroCopias { get; set; }
    public byte[] Conteudo { get; set; }
    [ForeignKey("ProfessorModel")]
    public int ProfessorId { get; set; }
    public TipoDaAvaliacao TipoDaAvaliacao { get; set; }
    public StatusDaProva StatusDaProva { get; set; }
    public TipoDeProva TipoDeProva { get; set; }
    public string ObservacaoDaProva { get; set; }
    public ProfessorModel Professor { get; set; }
    public Curso cursoProfessor { get; set; }
    public Disciplina disciplinaProfessor { get; set; }
    public Periodo periodoProfessor { get; set; }
}
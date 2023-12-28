using ProvaPortal.Models.Enum;

public class ProfessorDTO
{
    public int Id { get; set; }
    public int Matricula { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public Perfil Perfil { get; set; }
    public string UsuarioLogin { get; set; }
    public string NomeAbreviado { get; set; }
}

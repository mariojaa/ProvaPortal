using ProvaPortal.Models.Enum;

namespace ProvaPortal.Models.DTOs
{
    public class ProfessorDTO
    {
        public int Id { get; set; }
        public string NomeCompletoProfessor { get; set; }
        public string EmailAcademicoProfessor { get; set; }
        public int MatriculaProfessor { get; set; }
        public string NomeProfessorAbreviado { get; set; }
        public Perfil Perfil { get; set; }

    }
}

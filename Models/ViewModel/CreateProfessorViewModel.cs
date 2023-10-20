using Microsoft.AspNetCore.Mvc.Rendering;
using ProvaPortal.Models.Enum;

namespace ProvaPortal.Models.ViewModel
{
    public class CreateProfessorViewModel
    {
        public int Matricula { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public Perfil Perfil { get; set; }
        public string SenhaProfessor { get; set; }
        public string UsuarioLogin { get; set; }
        public int ProfessorId { get; set; }
        public ProfessorModel Professor { get; set; }

        public List<SelectListItem> CursoOptions { get; set; }
        public List<SelectListItem> PeriodoOptions { get; set; }
        public List<SelectListItem> DisciplinaOptions { get; set; }

        public Curso Curso { get; set; }
        public Periodo Periodo { get; set; }
        public Disciplina Disciplina { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProvaPortal.Models
{
    public class ProfessorModel
    {
        public int Id { get; set; }
        public int Matricula { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string SenhaProfessor { get; set; }

        [NotMapped]
        [Compare("SenhaProfessor", ErrorMessage = "Senhas não conferem! Verifique e tente novamente.")]
        public string ConfirmarSenhaProfessor { get; set; }

    }
}

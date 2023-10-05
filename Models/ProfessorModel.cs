using ProvaPortal.Filters;
using ProvaPortal.Helper;
using ProvaPortal.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace ProvaPortal.Models
{
    
    public class ProfessorModel
    {
        public int Id { get; set; }
        public int Matricula { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public Perfil Perfil { get; set; }
        public string SenhaProfessor { get; set; }
        public string CursoProfessor { get; set; }
        public int PeriodoProfessor { get; set; }
        public string TurmaProfessor { get; set; }
        public string UsuarioLogin { get; set; }
        [NotMapped]
        [Compare("SenhaProfessor", ErrorMessage = "Senhas não conferem! Verifique e tente novamente.")]
        public string ConfirmarSenhaProfessor { get; set; }



        public bool SenhaValida(string senha)
        {
            return SenhaProfessor == senha.GerarHash();
        }

        public void SetSenhaHash()
        {
            SenhaProfessor = SenhaProfessor.GerarHash();
        }
        public void SetNovaSenha(string novaSenha)
        {
            SenhaProfessor = novaSenha.GerarHash();
        }
        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            SenhaProfessor = novaSenha.GerarHash();
            return novaSenha;
        }

    }
}

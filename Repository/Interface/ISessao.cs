using ProvaPortal.Models;

namespace ProvaPortal.Repository.Interface
{
    public interface ISessao
    {
        void CriarSessaoUsuario(ProfessorDTO professorDTO);
        void RemoverSessaoUsuario();
        string BuscarDadosDaSessaoParaNomearArquivo(int numeroCopias);
        ProfessorModel BuscarSessaoUsuario();
       string BuscarSessaoDoUsuarioParaEnviarEmail(string email);
    }
}

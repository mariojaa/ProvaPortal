using ProvaPortal.Models;

namespace ProvaPortal.Helper
{
    public interface ISessao
    {
        void CriarSessaoUsuario(ProfessorModel professorModel);
        void RemoverSessaoUsuario();
        string BuscarUsuarioLoginNaSessao();
        ProfessorModel BuscarSessaoUsuario();
    }
}

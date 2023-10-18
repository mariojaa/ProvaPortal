using Newtonsoft.Json;
using ProvaPortal.Models;
using ProvaPortal.Repository.Interface;

namespace ProvaPortal.Repository
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ProfessorModel BuscarSessaoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoUsuarioLogado");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            
            return JsonConvert.DeserializeObject<ProfessorModel>(sessaoUsuario);
        }

        public void CriarSessaoUsuario(ProfessorModel usuario)
        {
            string valor = JsonConvert.SerializeObject(usuario);
            _httpContext.HttpContext.Session.SetString("sessaoUsuarioLogado", valor);
        }

        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoUsuarioLogado");
        }

        public string BuscarDadosDaSessaoParaNomearArquivo()
        {
            ProfessorModel dadosDoCursoDoProfessorNaSessao = BuscarSessaoUsuario();
            if(dadosDoCursoDoProfessorNaSessao != null)
            {
                return dadosDoCursoDoProfessorNaSessao.UsuarioLogin + "_" + dadosDoCursoDoProfessorNaSessao.CursoProfessor + "_" +
                    dadosDoCursoDoProfessorNaSessao.PeriodoProfessor + "_" + "Periodo" + "_" + "Turma" + "_" + dadosDoCursoDoProfessorNaSessao.TurmaProfessor;
            }
            return null;
        }    
    }
}

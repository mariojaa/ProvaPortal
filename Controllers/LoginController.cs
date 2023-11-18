using ProvaPortal.Models;
using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Repository.Interface;

namespace ProvaPortal.Controllers
{
    public class LoginController : Controller
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly ISessao _sessao;
        public LoginController(IProfessorRepository professorRepository, ISessao sessao)
        {
            _professorRepository = professorRepository;
            _sessao = sessao;
        }
        public IActionResult Index()
        {
            // quando logado redirecionar para lista contatos
            if (_sessao.BuscarSessaoUsuario() != null)
            {
                return RedirectToAction("ListarProfessores", "Professor");
            }

            return View();
        }
        //[LogActionFilter]
        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult RedefinirSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[LogActionFilter]
        public IActionResult Entrar(LoginModel loginmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProfessorModel usuarios = _professorRepository.BuscarPorLogin(loginmodel.Login);
                    if (usuarios != null)
                    {
                        if (usuarios.SenhaValida(loginmodel.Senha))
                        {
                            _sessao.CriarSessaoUsuario(usuarios);
                            return RedirectToAction("Index", "Provas");
                        }

                        TempData["MensagemErro"] = "Ops, Senha incorreta. Verifique e tente novamente.";

                    }
                    TempData["MensagemErro"] = "Ops, Usuario ou Senha incorreto. Verifique e tente novamente.";
                }
                TempData["MensagemErro"] = "Ops, Usuario ou Senha incorreto. Verifique e tente novamente.";
                return View("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return RedirectToAction("Erro", "Login");
            }
        }
    }
}

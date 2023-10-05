using ProvaPortal.Helper;
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
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

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
                            return RedirectToAction("Index", "Home");
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult EnviarLinkRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            UsuarioModel usuarios = _usuarioRepositorio.BuscarPorEmailELogin(redefinirSenhaModel.Email, redefinirSenhaModel.LoginId);
        //            if (usuarios != null)
        //            {
        //                string novaSenha = usuarios.GerarNovaSenha();

        //                string mensagem = $"Sua nova senha é: {novaSenha}";
        //                bool emailEnviado = _email.EnviarEmail(usuarios.Email, "Senha do Burro!", mensagem);

        //                if (emailEnviado)
        //                {
        //                    _usuarioRepositorio.Update(usuarios);
        //                    TempData["MensagemSucesso"] = "Enviamos para seu email cadastrado, uma nova senha para acesso.";
        //                }
        //                else
        //                {
        //                    TempData["MensagemErro"] = "Ops, Não conseguimos enviar o email. Verifique o email informado.";
        //                }

        //                return RedirectToAction("Index", "Login");
        //            }
        //            TempData["MensagemErro"] = "Ops, Não conseguimos redefinir sua senha. Verifique os dados informados.";
        //        }
        //        return RedirectToAction("RedefinirSenha", "Login");
        //    }
        //    catch (Exception)
        //    {
        //        TempData["MensagemErro"] = "Não foi possível redefinir sua tenha. Tente Novamente.";
        //        return RedirectToAction("Index", "Login");
        //    }
        //}
    }
}

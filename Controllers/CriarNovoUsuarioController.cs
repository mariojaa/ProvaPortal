using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Models;
//Delete from Usuarios where Id = 10;
namespace ProvaPortal.Controllers
{
    public class CriarNovoUsuarioController : Controller
    {
        private readonly ProfessorRepository _professorRepository;
        public CriarNovoUsuarioController(ProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public IActionResult CriarNovoUsuarioDeslogado()
        {
            try
            {
                return View();
            }
            catch
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarNovoUsuarioDeslogado(ProfessorModel professor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(professor.SenhaProfessor == professor.ConfirmarSenhaProfessor)
                    {
                        TempData["MensagemSucesso"] = "Novo usuário adicionado com sucesso!";
                        _professorRepository.AddProfessor(professor);
                        return RedirectToAction("Index", "Login");
                    }

                    TempData["MensagemErro"] = "Senha não confere! Verifique as senhas e tente novamente.";
                }
                return View();
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Usuario");
            }

        }

    }
}

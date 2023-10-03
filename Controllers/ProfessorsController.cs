using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Models;
using ProvaPortal.Repository;

namespace ProvaPortal.Controllers
{
    public class ProfessorsController : Controller
    {
        private readonly ProfessorRepository _professorRepository;
        public ProfessorsController(ProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }
        public IActionResult ListarProfessores()
        {
            try
            {

                return View(_professorRepository.BuscarTodosProfessores());
            }

            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Professors");
            }

        }
        public IActionResult CriarProfessor()
        {
            try
            {
                return View();
            }
            catch
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Professors");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarProfessor(ProfessorModel professorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    TempData["MensagemSucesso"] = "Professor adicionado com sucesso!";
                    _professorRepository.Adicionar(professorModel);
                    return RedirectToAction("ListarProfessores", "Professors");
                }
                return View();
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Professors");
            }

        }
        public IActionResult DeletarProfessor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _professorRepository.BuscarPorId(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletarProfessor(int id)
        {
            try
            {
                TempData["MensagemSucesso"] = "Professor excluido com sucesso!";
                _professorRepository.Remover(id);
                return RedirectToAction("ListarProfessores", "Professors");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Professors");
            }
        }
        /*public IActionResult DetalhesUsuario(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _usuarioRepositorio.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return PartialView(obj);
        }*/

        public IActionResult EditarProfessor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _professorRepository.BuscarPorId(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarProfessor(ProfessorModel professorModel)
        {
            try
            {
                TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
                _professorRepository.Update(professorModel);
                return RedirectToAction("ListarProfessores", "Professors");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("index");
            }
        }
    }
}

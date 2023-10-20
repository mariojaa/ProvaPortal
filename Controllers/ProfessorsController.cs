//using Microsoft.AspNetCore.Mvc;
//using ProvaPortal.Data;
//using ProvaPortal.Filters;
//using ProvaPortal.Models;
//using ProvaPortal.Repository;
//using ProvaPortal.Repository.Interface;

//namespace ProvaPortal.Controllers
//{
//    [PaginaSomenteAdmin]
//    public class ProfessorsController : Controller
//    {
//        private readonly ProfessorRepository _professorRepository;
//        private readonly IProvaRepository _provaRepository;
//        private readonly ProvaPortalContext _context;
//        private readonly ISessao _sessao;

//        public ProfessorsController(ProfessorRepository professorRepository, ProvaPortalContext provaPortalContext, ISessao sessao, IProvaRepository provaRepository)
//        {
//            _professorRepository = professorRepository;
//            _context = provaPortalContext;
//            _sessao = sessao;
//            _provaRepository = provaRepository;
//        }
//        public IActionResult ListarProfessores()
//        {
//            try
//            {
//                ProfessorModel professorLogado = _sessao.BuscarSessaoUsuario();
//                List<ProvaModel> provas = _provaRepository.ObterTodasProvas(professorLogado.Id);
//                return View(provas);
//            }

//            catch (Exception)
//            {
//                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
//                return View("Erro", "Professors");
//            }

//        }
//        public IActionResult CriarProfessor()
//        {
//            try
//            {
//                return View();
//            }
//            catch
//            {
//                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
//                return View("Erro", "Professors");
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult CriarProfessor(ProfessorModel professorModel)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {

//                    TempData["MensagemSucesso"] = "Professor adicionado com sucesso!";
//                    _professorRepository.Adicionar(professorModel);
//                    return RedirectToAction("ListarProfessores", "Professors");
//                }
//                return View();
//            }
//            catch (Exception)
//            {
//                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
//                return View("Erro", "Professors");
//            }

//        }
//        public IActionResult DeletarProfessor(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }
//            var obj = _professorRepository.BuscarPorId(id.Value);
//            if (obj == null)
//            {
//                return NotFound();
//            }
//            return PartialView(obj);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult DeletarProfessor(int id)
//        {
//            try
//            {
//                TempData["MensagemSucesso"] = "Professor excluido com sucesso!";
//                _professorRepository.Remover(id);
//                return RedirectToAction("ListarProfessores", "Professors");
//            }
//            catch (Exception)
//            {
//                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
//                return View("Erro", "Professors");
//            }
//        }
//        public IActionResult EditarProfessor(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }
//            var obj = _professorRepository.BuscarPorId(id.Value);
//            if (obj == null)
//            {
//                return NotFound();
//            }
//            return View(obj);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult EditarProfessor(ProfessorModel professorModel)
//        {
//            try
//            {
//                TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
//                _professorRepository.Update(professorModel);
//                return RedirectToAction("ListarProfessores", "Professors");
//            }
//            catch (Exception)
//            {
//                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
//                return View("index");
//            }
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProvaPortal.Models;
using ProvaPortal.Models.Enum;
using ProvaPortal.Repository.Interface;
using ProvaPortal.Data;
using ProvaPortal.Models.ViewModel;
using ProvaPortal.Filters;

[PaginaSomenteAdmin]
public class ProfessorController : Controller
{
    private readonly IProfessorRepository _professorRepository;
    private readonly ProvaPortalContext _context;

    public ProfessorController(IProfessorRepository professorRepository, ProvaPortalContext context)
    {
        _professorRepository = professorRepository;
        _context = context;
    }

    public IActionResult Create()
    {
        var cursoOptions = Enum.GetValues(typeof(Curso))
            .Cast<Curso>()
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.ToString()
            })
            .ToList();

        var periodoOptions = Enum.GetValues(typeof(Periodo))
            .Cast<Periodo>()
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.ToString()
            })
            .ToList();

        var disciplinaOptions = Enum.GetValues(typeof(Disciplina))
            .Cast<Disciplina>()
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.ToString()
            })
            .ToList();

        var viewModel = new CreateProfessorViewModel
        {
            CursoOptions = cursoOptions,
            PeriodoOptions = periodoOptions,
            DisciplinaOptions = disciplinaOptions
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateProfessorViewModel model)
    {
        if (ModelState.IsValid)
        {
            var professor = new ProfessorModel
            {
                Matricula = model.Matricula,
                NomeCompleto = model.NomeCompleto,
                Email = model.Email,
                Perfil = model.Perfil,
                SenhaProfessor = model.SenhaProfessor,
                UsuarioLogin = model.UsuarioLogin
            };

            _professorRepository.AddProfessor(professor);

            return RedirectToAction("ListarProfessores");
        }
        return View(model);
    }
    public IActionResult ListarProfessores()
    {
        var professores = _professorRepository.GetAllProfessores();
        return View(professores);
    }
}

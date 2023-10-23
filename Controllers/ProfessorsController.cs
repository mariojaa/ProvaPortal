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

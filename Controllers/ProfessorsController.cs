using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProvaPortal.Models;
using ProvaPortal.Models.Enum;
using ProvaPortal.Repository.Interface;
using ProvaPortal.Models.ViewModel;
using ProvaPortal.Filters;

namespace ProvaPortal.Controllers
{
    [PaginaSomenteAdmin]
    public class ProfessorController : Controller
    {
        private readonly IProfessorRepository _professorRepository;

        public ProfessorController(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(CreateProfessorViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var professor = new ProfessorModel
        //        {
        //            Matricula = model.Matricula,
        //            NomeCompleto = model.NomeCompleto,
        //            Email = model.Email,
        //            Perfil = model.Perfil,
        //            SenhaProfessor = model.SenhaProfessor,
        //            UsuarioLogin = model.UsuarioLogin
        //        };

        //        _professorRepository.AddProfessor(professor);

        //        return RedirectToAction("ListarProfessores");
        //    }
        //    return View(model);
        //}
        public IActionResult ListarProfessores()
        {
            var professores = _professorRepository.GetAllProfessores();
            return View(professores);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateProfessorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nomeAbreviado = Abreviate(model.NomeCompleto);

                var professor = new ProfessorModel
                {
                    Matricula = model.Matricula,
                    NomeCompleto = model.NomeCompleto,
                    Email = model.Email,
                    Perfil = model.Perfil,
                    SenhaProfessor = model.SenhaProfessor,
                    UsuarioLogin = model.UsuarioLogin,
                    //NomeProfessor = nomeAbreviado // Adicionando o nome abreviado à nova coluna
                };

                _professorRepository.AddProfessor(professor);

                return RedirectToAction("ListarProfessores");
            }
            return View(model);
        }

        // Método para abreviar o nome
        public static string Abreviate(string nome)
        {
            var meio = " ";
            var nomes = nome.Split(' ');
            for (var i = 1; i < nomes.Length - 1; i++)
            {
                if (!nomes[i].Equals("de", StringComparison.OrdinalIgnoreCase) &&
                    !nomes[i].Equals("da", StringComparison.OrdinalIgnoreCase) &&
                    !nomes[i].Equals("do", StringComparison.OrdinalIgnoreCase) &&
                    !nomes[i].Equals("das", StringComparison.OrdinalIgnoreCase) &&
                    !nomes[i].Equals("dos", StringComparison.OrdinalIgnoreCase))
                    meio += nomes[i][0] + ". ";
            }
            return nomes[0] + meio + nomes[nomes.Length - 1];
        }
    }
}
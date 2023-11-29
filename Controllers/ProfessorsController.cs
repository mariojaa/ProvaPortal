using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProvaPortal.Models;
using ProvaPortal.Models.Enum;
using ProvaPortal.Repository.Interface;
using ProvaPortal.Models.ViewModel;
using ProvaPortal.Filters;

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
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult EnviarEmailConfirmacaoDeEnvioDeProva(ProfessorModel redefinirSenhaModel)
    //{
    //    try
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            ProfessorModel professores = _professorRepository(redefinirSenhaModel.Email, redefinirSenhaModel.UsuarioLogin);
    //            if (professores != null)
    //            {
    //                string mensagem = $"Prova enviada com sucesso!";
    //                bool emailEnviado = _email.EnviarEmail(professores.Email, "Senha do Burro!", mensagem);

    //                if (emailEnviado)
    //                {
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

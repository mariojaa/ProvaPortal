using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using ProvaPortal.Data;
using ProvaPortal.Filters;
using ProvaPortal.Models;
using ProvaPortal.Repository;
using ProvaPortal.Repository.Interface;
using System.Security.Claims;

namespace ProvaPortal.Controllers
{
    [PaginaUsuarioLogado]
    public class ProvasController : Controller
    {
        private readonly IProvaRepository _provaRepository;
        private readonly ProvaPortalContext _context;
        private readonly ISessao _sessao;

        public ProvasController(IProvaRepository provaRepository, ISessao sessao, ProvaPortalContext context)
        {
            _provaRepository = provaRepository;
            _sessao = sessao;
            _context = context;
        }

        [HttpGet]
        public IActionResult EnviarProva()
        {

            DateTime ultimoAcesso = DateTime.Now;

            var prova = _provaRepository.ObterTodasProvas().FirstOrDefault();

            return View(prova);
        }

        [HttpPost]
        public IActionResult EnviarProva(IFormFile arquivo, int numeroCopias)
        {
            if (arquivo != null && arquivo.Length > 0)
            {
                string dadosSessaoProfessor = _sessao.BuscarDadosDaSessaoParaNomearArquivo();
                if (string.IsNullOrEmpty(dadosSessaoProfessor))
                {
                    return RedirectToAction("EnviarProva");
                }

                string nomeArquivo = $"{dadosSessaoProfessor}_{arquivo.FileName}_{numeroCopias}_Copias_.pdf";
                string nomeProvaOriginal = $"{arquivo.FileName}";
                string caminhoArquivo = Path.Combine("ArquivosProva", nomeArquivo);

                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    arquivo.CopyTo(fileStream);
                }

                var prova = new ProvaModel
                {
                    NumeroCopias = numeroCopias,
                    NomeArquivo = nomeProvaOriginal,
                    DataEnvio = DateTime.Now,
                };

                _provaRepository.AdicionarProva(prova);
                _context.Provas.Add(prova);
                _context.SaveChanges();

                return RedirectToAction("EnviarProva");
            }

            return RedirectToAction("EnviarProva");
        }
        public IActionResult Index()
        {
            try
            {

                return View(_provaRepository.ObterTodasProvas());
            }

            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Professors");
            }
        }
        public ActionResult MostrarDados()
        {
            ClaimsPrincipal user = HttpContext.User;
            string nomeArquivo = user.Identity.Name;
            var provaModel = new ProvaModel
            {
                NomeArquivo = nomeArquivo
            };
            return View("Index");
        }
    }
}
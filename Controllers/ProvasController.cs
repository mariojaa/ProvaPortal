using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Data;
using ProvaPortal.Filters;
using ProvaPortal.Models;
using ProvaPortal.Models.Enum;
using ProvaPortal.Repository.Interface;

namespace ProvaPortal.Controllers
{
    [PaginaUsuarioLogado]
    public class ProvasController : Controller
    {
        private readonly IProvaRepository _provaRepository;
        private readonly ISessao _sessao;
        private readonly IProfessorRepository _professorRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ProvaPortalContext _context;

        public ProvasController(IProvaRepository provaRepository, ISessao sessao,
            IProfessorRepository professorRepository, IWebHostEnvironment hostingEnvironment,
            ProvaPortalContext context)
        {
            _provaRepository = provaRepository;
            _sessao = sessao;
            _professorRepository = professorRepository;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [HttpGet]
        public IActionResult EnviarProva()
        {
            DateTime ultimoAcesso = DateTime.Now;
            return View();
        }

        [HttpPost]
        public IActionResult EnviarProva(IFormFile arquivo, int numeroCopias, string obsProva, Curso curso, TipoDaAvaliacao tipoDaAvaliacao, 
            TipoDeProva tipoDeProva)
        {
            if (arquivo != null && arquivo.Length > 0)
            {
                var professorLogado = _sessao.BuscarSessaoUsuario();
                if (professorLogado == null)
                {
                    return RedirectToAction("Login", "Index");
                }

                string dadosSessaoProfessor = _sessao.BuscarDadosDaSessaoParaNomearArquivo(numeroCopias);
                if (string.IsNullOrEmpty(dadosSessaoProfessor))
                {
                    return RedirectToAction("EnviarProva");
                }

                string nomeArquivo = $"{dadosSessaoProfessor}_{curso}_{arquivo.FileName}_{numeroCopias}_Copias_.pdf"; // Renomeia o arquivo
                string nomeProvaOriginal = $"{arquivo.FileName}";
                string caminhoArquivo = Path.Combine("ArquivosProva", nomeArquivo);

                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    arquivo.CopyTo(fileStream);
                }

                // Lê o conteúdo do arquivo em um array de bytes
                byte[] conteudoArquivo;
                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read))
                {
                    conteudoArquivo = new byte[fileStream.Length];
                    fileStream.Read(conteudoArquivo, 0, (int)fileStream.Length);
                }

                var prova = new ProvaModel
                {
                    NumeroCopias = numeroCopias,
                    NomeArquivo = nomeProvaOriginal,
                    DataEnvio = DateTime.Now,
                    ProfessorId = professorLogado.Id,
                    ObservacaoDaProva = string.IsNullOrEmpty(obsProva) ? "" : obsProva,
                    Conteudo = conteudoArquivo,
                    TipoDaAvaliacao = tipoDaAvaliacao,
                    TipoDeProva = tipoDeProva
                };

                _provaRepository.AdicionarProva(prova);
                return RedirectToAction("EnviarProva");
            }

            return RedirectToAction("EnviarProva");
        }

        public IActionResult Index()
        {
            try
            {
                ProfessorModel professorLogado = _sessao.BuscarSessaoUsuario();
                if (professorLogado == null)
                {
                    return RedirectToAction("Login", "Index");
                }

                List<ProvaModel> provas = _provaRepository.ObterTodasProvas(professorLogado.Id);

                return View(provas);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Provas");
            }
        }

        public IActionResult DeletarProva(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _provaRepository.BuscarProvaPorId(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return PartialView(obj);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletarProva(int id)
        //{
        //    try
        //    {
        //        TempData["MensagemSucesso"] = "Prova excluída com sucesso!";
        //        _provaRepository.DeleteProva(id);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception)
        //    {
        //        TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
        //        return View("Erro", "Provas");
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletarProva(int id)
        {
            try
            {
                var prova = _provaRepository.BuscarProvaPorId(id);

                if (prova == null)
                {
                    return Json(new { success = false, error = "Prova não encontrada." });
                }

                prova.StatusDaProva = StatusDaProva.Deletado; // Marca a prova como "Deletada" no banco de dados
                _provaRepository.AtualizarProva(prova);

                return Json(new { success = true, message = "Prova marcada como deletada." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Ocorreu um erro ao marcar a prova como deletada." });
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(PaginaSomenteAdmin))]
        public IActionResult VisualizarTodasProvas()
        {
            try
            {
                ProfessorModel professorLogado = _sessao.BuscarSessaoUsuario();
                if (professorLogado == null)
                {
                    return RedirectToAction("Login", "Index");
                }

                //dados do professor relacionado às provas
                List<ProvaModel> todasAsProvas = _provaRepository.ObterTodasProvasAdministradorComProfessores();

                return View(todasAsProvas);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Provas");
            }
        }

        public IActionResult VisualizarProva(int id)
        {
            var prova = _provaRepository.BuscarProvaPorId(id);

            if (prova == null)
            {
                return NotFound();
            }

            byte[] arquivoPDF = prova.Conteudo;

            if (arquivoPDF == null || arquivoPDF.Length == 0)
            {
                return NotFound();
            }

            return File(arquivoPDF, "application/pdf");
        }
        [HttpPost]
        public IActionResult AtualizarStatusImpresso(int id)
        {
            try
            {
                var prova = _provaRepository.BuscarProvaPorId(id);

                if (prova == null)
                {
                    return Json(new { success = false, error = "Prova não encontrada." });
                }

                prova.StatusDaProva = StatusDaProva.Impresso; // Define status para "Impresso"
                                                              // Atualiza prova no banco de dados
                _provaRepository.AtualizarProva(prova);

                // visualiza prova
                return Json(new { success = true, urlParaProva = Url.Action("VisualizarProva", "Provas", new { id = id }) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Ocorreu um erro ao atualizar o status da prova." });
            }
        }

    }
}

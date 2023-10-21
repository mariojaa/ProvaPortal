using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Data;
using ProvaPortal.Filters;
using ProvaPortal.Models;
using ProvaPortal.Models.Enum;
using ProvaPortal.Repository.Interface;
using Microsoft.AspNetCore.Hosting;


namespace ProvaPortal.Controllers
{
    [PaginaUsuarioLogado]
    public class ProvasController : Controller
    {
        private readonly IProvaRepository _provaRepository;
        private readonly ProvaPortalContext _context;
        private readonly ISessao _sessao;
        private readonly IProfessorRepository _professorRepository;
        private readonly PaginaSomenteAdmin _paginaSomenteAdmin;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProvasController(IProvaRepository provaRepository, IWebHostEnvironment hostingEnvironment, PaginaSomenteAdmin paginaSomenteAdmin, ISessao sessao, ProvaPortalContext context, IProfessorRepository professorRepository)
        {
            _provaRepository = provaRepository;
            _sessao = sessao;
            _context = context;
            _professorRepository = professorRepository;
            _paginaSomenteAdmin = paginaSomenteAdmin;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult EnviarProva()
        {

            DateTime ultimoAcesso = DateTime.Now;
            return View();
        }

        [HttpPost]
        public IActionResult EnviarProva(IFormFile arquivo, int numeroCopias, string obsProva, Curso curso)
        {
            if (arquivo != null && arquivo.Length > 0)
            {
                var professorId = _sessao.BuscarSessaoUsuario();
                string dadosSessaoProfessor = _sessao.BuscarDadosDaSessaoParaNomearArquivo(numeroCopias);
                if (string.IsNullOrEmpty(dadosSessaoProfessor))
                {
                    return RedirectToAction("EnviarProva");
                }

                // Lê o conteúdo do arquivo em um array de bytes
                byte[] conteudoArquivo;
                using (var ms = new MemoryStream())
                {
                    arquivo.CopyTo(ms);
                    conteudoArquivo = ms.ToArray();
                }

                string nomeArquivo = $"{dadosSessaoProfessor}_{curso}_{arquivo.FileName}_{numeroCopias}_Copias_.pdf"; // Renomeia o arquivo
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
                    ProfessorId = professorId.Id,
                    ObservacaoDaProva = string.IsNullOrEmpty(obsProva) ? "" : obsProva,
                    Conteudo = conteudoArquivo // Atribui o conteúdo do arquivo ao campo Conteudo
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletarProva(int id)
        {
            try
            {
                TempData["MensagemSucesso"] = "Professor excluido com sucesso!";
                _provaRepository.DeleteProva(id);
                return RedirectToAction("Index", "Provas");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Provas");
            }
        }
        [HttpGet]
        [ServiceFilter(typeof(PaginaSomenteAdmin))]
        public IActionResult VisualizarTodasProvas()
        {
            var usuario = _sessao.BuscarSessaoUsuario();

            if (usuario.Perfil == Models.Enum.Perfil.Administrador)
            {
                List<ProvaModel> todasAsProvas = _provaRepository.ObterTodasProvasAdministrador();
                return View(todasAsProvas);
            }
            else
            {
                // Professor: Redirecionar ou mostrar mensagem de erro, conforme necessário
                return RedirectToAction("AcessoNegado"); // Redirecione para uma página de acesso negado, por exemplo
            }
        }
        public IActionResult VisualizarProva(int id)
        {
            var prova = _provaRepository.BuscarProvaPorId(id);

            if (prova == null)
            {
                return NotFound();
            }

            // Obtém o conteúdo do arquivo PDF do campo Conteudo
            byte[] arquivoPDF = prova.Conteudo;

            if (arquivoPDF == null || arquivoPDF.Length == 0)
            {
                return NotFound();
            }

            // Gere uma resposta para o arquivo PDF
            return File(arquivoPDF, "application/pdf");
        }
        [HttpGet]
        public IActionResult ImprimirProva(int id)
        {
            var prova = _provaRepository.BuscarProvaPorId(id);

            if (prova == null)
            {
                return NotFound();
            }

            // Certifique-se de que seu modelo de dados contenha o campo correto que armazena o conteúdo do arquivo PDF
            byte[] arquivoPDF = prova.Conteudo;

            if (arquivoPDF == null || arquivoPDF.Length == 0)
            {
                return NotFound();
            }

            // Gere uma resposta para o arquivo PDF com o cabeçalho para impressão
            Response.Headers["Content-Disposition"] = "inline; filename=Prova.pdf";
            return File(arquivoPDF, "application/pdf");
        }


    }
}
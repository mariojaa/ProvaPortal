using DinkToPdf;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProvaPortal.Data;
using ProvaPortal.Extensions;
using ProvaPortal.Filters;
using ProvaPortal.Models;
using ProvaPortal.Models.Enum;
using ProvaPortal.Repository.Interface;
using ProvaPortal.SessaoUsuario;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using PaperKind = DinkToPdf.PaperKind;

namespace ProvaPortal.Controllers
{
    [PaginaUsuarioLogado]
    public class ProvasController : Controller
    {
        private readonly IProvaRepository _provaRepository;
        private readonly ISessao _sessao;
        private readonly IEmail _email;
        private readonly ProvaPortalContext _context;

        public ProvasController(
            IProvaRepository provaRepository,
            ISessao sessao,
            IEmail email,
            ProvaPortalContext context)
        {
            _provaRepository = provaRepository;
            _sessao = sessao;
            _email = email;
            _context = context;
        }

        [HttpGet]
        public IActionResult EnviarProva()
        {
            DateTime ultimoAcesso = DateTime.Now;
            return View();
        }

        //[HttpPost]
        //public IActionResult EnviarProva(IFormFile arquivo, int numeroCopias, string obsProva, Curso curso, TipoDaAvaliacao tipoDaAvaliacao,
        //    TipoDeProva tipoDeProva)
        //{
        //    if (arquivo != null && arquivo.Length > 0)
        //    {
        //        var professorLogado = _sessao.BuscarSessaoUsuario();
        //        if (professorLogado == null)
        //        {
        //            return RedirectToAction("Login", "Index");
        //        }

        //        string dadosSessaoProfessor = _sessao.BuscarDadosDaSessaoParaNomearArquivo(numeroCopias);
        //        if (string.IsNullOrEmpty(dadosSessaoProfessor))
        //        {
        //            return RedirectToAction("EnviarProva");
        //        }
        //        Guid guid = Guid.NewGuid();
        //        string nomeArquivo = guid.ToString();
        //        //string nomeArquivo = $"{dadosSessaoProfessor}_{curso}_{arquivo.FileName}_{numeroCopias}_Copias_.pdf"; // Renomeia o arquivo
        //        string nomeProvaOriginal = $"{arquivo.FileName}";
        //        string caminhoArquivo = Path.Combine("ArquivosProva", nomeArquivo);

        //        using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
        //        {
        //            arquivo.CopyTo(fileStream);
        //        }

        //        // Lê o conteúdo do arquivo em um array de bytes
        //        byte[] conteudoArquivo;
        //        using (var fileStream = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read))
        //        {
        //            conteudoArquivo = new byte[fileStream.Length];
        //            fileStream.Read(conteudoArquivo, 0, (int)fileStream.Length);
        //        }

        //        var prova = new ProvaModel
        //        {
        //            NumeroCopias = numeroCopias,
        //            NomeArquivo = nomeProvaOriginal,
        //            DataEnvio = DateTime.Now,
        //            ProfessorId = professorLogado.Id,
        //            ObservacaoDaProva = string.IsNullOrEmpty(obsProva) ? "" : obsProva,
        //            Conteudo = conteudoArquivo,
        //            TipoDaAvaliacao = tipoDaAvaliacao,
        //            TipoDeProva = tipoDeProva
        //        };
        //        if (ModelState.IsValid)
        //        {
        //            var enviarEmailProfessorLogado = _sessao.BuscarSessaoDoUsuarioParaEnviarEmail(professorLogado.Email);
        //            if (enviarEmailProfessorLogado != null)
        //            {

        //                string mensagem = $"Docente {professorLogado.UsuarioLogin}, sua prova {nomeProvaOriginal}, com {numeroCopias} cópias, foi enviada com sucesso!";
        //                bool emailEnviado = _email.EnviarEmail(enviarEmailProfessorLogado, "Prova enviada com sucesso!", mensagem);

        //                if (emailEnviado)
        //                {
        //                    TempData["MensagemSucesso"] = "Prova enviada com sucesso, você receverá um email em estantes!";
        //                    _provaRepository.AdicionarProva(prova);
        //                }
        //                else
        //                {
        //                    TempData["MensagemErro"] = "Ops, Não conseguimos enviar o email. Verifique o email informado.";
        //                }

        //                return RedirectToAction("Index", "Provas");
        //            }

        //            return RedirectToAction("Index");
        //        }
        //        return RedirectToAction("Index", "Provas");

        //    }

        //    return RedirectToAction("EnviarProva");
        //}
        //--------------novo metodo enviar prova com limite de pdf upload 20 megas----------------------

        [HttpPost]
        [RequestSizeLimit(20 * 1024 * 1024)] // 20MB limit
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

                Guid guid = Guid.NewGuid();
                string nomeArquivo = guid.ToString();

                if (arquivo.Length > 20 * 1024 * 1024)
                {
                    //TempData["MensagemTamanhoExcedido"] = "Tamanho do arquivo deve ser menor que 20MB.";
                    return RedirectToAction("EnviarProva");
                }

                string nomeProvaOriginal = $"{arquivo.FileName}";
                string caminhoArquivo = Path.Combine("ArquivosProva", nomeArquivo);

                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    arquivo.CopyTo(fileStream);
                }

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

                if (ModelState.IsValid)
                {
                    var enviarEmailProfessorLogado = _sessao.BuscarSessaoDoUsuarioParaEnviarEmail(professorLogado.Email);
                    if (enviarEmailProfessorLogado != null)
                    {
                        string mensagem = $"Docente {professorLogado.UsuarioLogin}, sua prova {nomeProvaOriginal}, com {numeroCopias} cópias, foi enviada com sucesso!";
                        bool emailEnviado = _email.EnviarEmail(enviarEmailProfessorLogado, "Prova enviada com sucesso!", mensagem);

                        if (emailEnviado)
                        {
                            TempData["MensagemSucesso"] = "Prova enviada com sucesso, você receberá um email em instantes!";
                            _provaRepository.AdicionarProva(prova);
                        }
                        else
                        {
                            TempData["MensagemErro"] = "Ops, Não conseguimos enviar o email. Verifique o email informado.";
                        }

                        return RedirectToAction("Index", "Provas");
                    }

                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", "Provas");

            }

            return RedirectToAction("EnviarProva");
        }


        //----------------------------------------------------------------------------------------------
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
                var prova = _provaRepository.BuscarProvaPorId(id);

                if (prova == null)
                {
                    TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                }

                if (prova.StatusDaProva.Equals(StatusDaProva.Deletado))
                {
                    if (ModelState.IsValid)
                    {
                        var professorLogado = _sessao.BuscarSessaoUsuario();
                        if (professorLogado == null)
                        {
                            return RedirectToAction("Login", "Index");
                        }
                        var enviarEmailProfessorLogado = _sessao.BuscarSessaoDoUsuarioParaEnviarEmail(professorLogado.Email);
                        if (enviarEmailProfessorLogado != null)
                        {
                            var link = "http://portal.ugb.edu.br/User/ForgotPassword?Length=4";
                            string mensagem = $"Docente {professorLogado.UsuarioLogin}, você acaba de deletar a prova {prova.NomeArquivo} de nosso sistema! Caso não foi você redefina sua senha através do link {link}";
                            bool emailEnviado = _email.EnviarEmail(enviarEmailProfessorLogado, "você acabou de deletar uma prova!", mensagem);

                            if (emailEnviado)
                            {
                                TempData["MensagemSucesso"] = "você acabou de deletar uma prova! Foi enviado um e-mail";
                                _provaRepository.DeleteProva(id);
                            }
                            else
                            {
                                TempData["MensagemErro"] = "Ops, Não conseguimos enviar o email. Verifique o email informado.";
                            }

                            return RedirectToAction("Index", "Provas");
                        }

                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                if (prova.StatusDaProva.Equals(StatusDaProva.Enviado))
                {
                    prova.StatusDaProva = StatusDaProva.Deletado;

                    _provaRepository.AtualizarProva(prova);
                    TempData["MensagemSucesso"] = "Prova excluída com sucesso!";
                }
                if (prova.StatusDaProva.Equals(StatusDaProva.AguardandoImpressao))
                {
                    prova.StatusDaProva = StatusDaProva.Deletado;
                    _provaRepository.AtualizarProva(prova);
                    TempData["MensagemSucesso"] = "Prova excluída com sucesso!";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                return View("Erro", "Provas");
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

        [ServiceFilter(typeof(PaginaSomenteAdmin))]
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
        [ServiceFilter(typeof(PaginaSomenteAdmin))]
        public IActionResult AtualizarStatusAguardandoImpressao(int id)
        {
            try
            {
                var prova = _provaRepository.BuscarProvaPorId(id);
                var emailProfessorProva = _provaRepository.ObterTodasProvasAdministradorComProfessores();
                var buscarEmailProfessorProvaImpressa = prova.Professor.Email;
                var administradorLogado = _sessao.BuscarSessaoUsuario();
                prova.StatusDaProva = StatusDaProva.AguardandoImpressao;
                _provaRepository.AtualizarProva(prova);
                //if (prova != null)
                //{
                //    string emailProfessor = prova.Professor.Email;

                //    prova.StatusDaProva = StatusDaProva.AguardandoImpressao;

                //    if (ModelState.IsValid)
                //    {

                //        if (buscarEmailProfessorProvaImpressa != null)
                //        {
                //            string mensagem = $"Docente {prova.Professor.UsuarioLogin}, sua prova {prova.NomeArquivo}, com {prova.NumeroCopias} cópias, acaba de ser acessada pelo Administrador: {administradorLogado.UsuarioLogin}. Em breve receberá um email de impressão de prova.";
                //            bool emailEnviado = _email.EnviarEmail(buscarEmailProfessorProvaImpressa, "Prova Acessada pela Mecanografia", mensagem);

                //            if (emailEnviado)
                //            {
                //                TempData["MensagemSucesso"] = $"Prova {prova.NomeArquivo}, com {prova.NumeroCopias} cópias, acaba de ser acessada!";
                //                _provaRepository.AtualizarProva(prova);
                //            }
                //            else
                //            {
                //                TempData["MensagemErro"] = "Ops, não conseguimos enviar o email. Verifique o email informado.";
                //            }

                //            return RedirectToAction("Index", "Provas");
                //        }
                //    }
                //}
                //else
                //{
                //    TempData["MensagemErro"] = "Professor não encontrado para esta prova ou o email do professor está vazio.";
                //}

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Ocorreu um erro ao atualizar o status da prova." });
            }
        }


        [HttpPost]
        [ServiceFilter(typeof(PaginaSomenteAdmin))]
        public IActionResult AtualizarStatusNotificada(List<int> ids)
        {
            try
            {
                foreach (var provaId in ids)
                {
                    var prova = _provaRepository.BuscarProvaPorId(provaId);
                    var emailProfessorProva = _provaRepository.ObterTodasProvasAdministradorComProfessores();
                    var buscarEmailProfessorProvaImpressa = prova.Professor.Email;
                    var administradorLogado = _sessao.BuscarSessaoUsuario();

                    if (prova != null)
                    {
                        string emailProfessor = prova.Professor.Email;
                        prova.StatusDaProva = StatusDaProva.Impressa;

                        if (ModelState.IsValid)
                        {
                            if (buscarEmailProfessorProvaImpressa != null)
                            {
                                string mensagem = $"Docente {prova.Professor.UsuarioLogin}, sua prova {prova.NomeArquivo}, acaba de ser impressa com {prova.NumeroCopias} cópias, pelo Administrador: {administradorLogado.UsuarioLogin}.";
                                bool emailEnviado = _email.EnviarEmail(buscarEmailProfessorProvaImpressa, "Prova Impressa!", mensagem);

                                if (emailEnviado)
                                {
                                    TempData["MensagemSucesso"] = $"Prova {prova.NomeArquivo}, acaba de ser impressa com {prova.NumeroCopias}!";
                                    _provaRepository.AtualizarProva(prova);
                                }
                                else
                                {
                                    TempData["MensagemErro"] = "Ops, não conseguimos enviar o email. Verifique o email informado.";
                                }
                            }
                        }
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Professor não encontrado para esta prova ou o email do professor está vazio.";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Ocorreu um erro ao atualizar o status da prova." });
            }
        }

        [HttpPost]
        public IActionResult ReverterStatusProva(int id)
        {
            try
            {
                var prova = _context.Provas.Find(id);
                if (prova != null)
                {
                    prova.StatusDaProva = StatusDaProva.Enviado;
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult ReverterStatusProvaImpressa(int id)
        {
            try
            {
                var prova = _context.Provas.Find(id);
                if (prova != null)
                {
                    prova.StatusDaProva = StatusDaProva.Enviado;
                    _context.SaveChanges();
                }

                return RedirectToAction("Index", "Provas");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return RedirectToAction("Index", "Provas");
            }
        }
        [HttpPost]
        public ActionResult PiscarProvaRestaurada(int id) //não funcional
        {

            ViewBag.RestaurarProvaId = id;

            return Json(new { success = true });
        }
        public IActionResult MoverParaDeletadas(int id)
        {
            try
            {

                var prova = _provaRepository.BuscarProvaPorId(id);
                prova.StatusDaProva = StatusDaProva.Deletado;
                _provaRepository.AtualizarProva(prova);

                TempData["MensagemSucesso"] = "Prova movida para Deletadas com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao mover a prova para Deletadas.";
            }

            return RedirectToAction("Index");
        }

        //---------Teste Impressão -----------------------
        //[HttpPost]
        //[ServiceFilter(typeof(PaginaSomenteAdmin))]
        //public IActionResult ImprimirProva([FromBody] ImprimirProvaRequest request)
        //{
        //    try
        //    {
        //        var prova = _provaRepository.BuscarProvaPorId(request.ProvaId);
        //        if (prova != null)
        //        {

        //            var converter = new BasicConverter(new PdfTools());

        //            var doc = new HtmlToPdfDocument()
        //            {
        //                GlobalSettings = {
        //            ColorMode = DinkToPdf.ColorMode.Color,
        //            Orientation = Orientation.Portrait,
        //            PaperSize = PaperKind.A4,
        //        },
        //                Objects = {
        //            new ObjectSettings
        //            {
        //                HtmlContent = "<h1>Conteúdo da prova aqui</h1>",
        //            }
        //        }
        //            };

        //            var pdfBytes = converter.Convert(doc);

        //            // Lógica para enviar o PDF para a impressora padrão do usuário.
        //            // (Dependente do sistema operacional e configurações específicas.)

        //            return Json(new { success = true });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, error = "Prova não encontrada." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, error = "Ocorreu um erro ao imprimir a prova." });
        //    }
        //}

    }
}
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Data;
using ProvaPortal.Filters;
using ProvaPortal.Models;
using ProvaPortal.Models.Enum;
using ProvaPortal.Repository.Interface;
using ProvaPortal.SessaoUsuario;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;

namespace ProvaPortal.Controllers
{
    [PaginaUsuarioLogado]
    public class ProvasController : Controller
    {
        private readonly IProvaRepository _provaRepository;
        private readonly ISessao _sessao;
        private readonly IEmail _email;

        public ProvasController(
            IProvaRepository provaRepository,
            ISessao sessao,
            IEmail email)
        {
            _provaRepository = provaRepository;
            _sessao = sessao;
            _email = email;
        }

        [HttpGet]
        public IActionResult EnviarProva()
        {
            DateTime ultimoAcesso = DateTime.Now;
            return View();
        }

        [HttpPost]
        //[LogActionFilter]
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
                //string nomeArquivo = $"{dadosSessaoProfessor}_{curso}_{arquivo.FileName}_{numeroCopias}_Copias_.pdf"; // Renomeia o arquivo
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
                if (ModelState.IsValid)
                {
                    var enviarEmailProfessorLogado = _sessao.BuscarSessaoDoUsuarioParaEnviarEmail(professorLogado.Email);
                    if (enviarEmailProfessorLogado != null)
                    {

                        string mensagem = $"Docente {professorLogado.UsuarioLogin}, sua prova {nomeProvaOriginal}, com {numeroCopias} cópias, foi enviada com sucesso!";
                        bool emailEnviado = _email.EnviarEmail(enviarEmailProfessorLogado, "Prova enviada com sucesso!", mensagem);

                        if (emailEnviado)
                        {
                            TempData["MensagemSucesso"] = "Prova enviada com sucesso, você receverá um email em estantes!";
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
        //[LogActionFilter]
        public IActionResult DeletarProva(int id)
        {
            try
            {
                var prova = _provaRepository.BuscarProvaPorId(id);

                if (prova == null)
                {
                    TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
                }

                if (prova.StatusDaProva == StatusDaProva.Deletado)
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
                if (prova.StatusDaProva == StatusDaProva.Enviado)
                {
                    prova.StatusDaProva = StatusDaProva.Deletado;

                    _provaRepository.AtualizarProva(prova);
                    TempData["MensagemSucesso"] = "Prova excluída com sucesso!";
                }
                if (prova.StatusDaProva == StatusDaProva.Impresso)
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
        //public IActionResult DeletarProvaAdministrador(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var obj = _provaRepository.BuscarProvaPorId(id.Value);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return PartialView(obj);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ServiceFilter(typeof(PaginaSomenteAdmin))]
        ////[LogActionFilter]
        //public IActionResult DeletarProvaAdministrador(int id)
        //{
        //    try
        //    {
        //        var prova = _provaRepository.BuscarProvaPorId(id);

        //        if (prova == null)
        //        {
        //            TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
        //        }

        //        if (prova.StatusDaProva == StatusDaProva.Deletado)
        //        {
        //            _provaRepository.DeleteProva(id);
        //            TempData["MensagemSucesso"] = "Prova excluída com sucesso!";
        //        }
        //        if (prova.StatusDaProva == StatusDaProva.Enviado)
        //        {
        //            prova.StatusDaProva = StatusDaProva.Deletado;
        //            _provaRepository.AtualizarProva(prova);
        //            TempData["MensagemSucesso"] = "Prova excluída com sucesso!";
        //        }
        //        if (prova.StatusDaProva == StatusDaProva.Impresso)
        //        {
        //            prova.StatusDaProva = StatusDaProva.Deletado;
        //            _provaRepository.AtualizarProva(prova);
        //            TempData["MensagemSucesso"] = "Prova excluída com sucesso!";
        //        }
        //        return RedirectToAction("VisualizarTodasProvas");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["MensagemErro"] = "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.";
        //        return View("Erro", "Provas");
        //    }
        //}
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
        public IActionResult AtualizarStatusImpresso(int id)
        {
            try
            {
                var prova = _provaRepository.BuscarProvaPorId(id);
                var emailProfessorProva = _provaRepository.ObterTodasProvasAdministradorComProfessores();
                var buscarEmailProfessorProvaImpressa = prova.Professor.Email;
                var administradorLogado = _sessao.BuscarSessaoUsuario();
                if (prova != null)
                {
                    string emailProfessor = prova.Professor.Email;

                    prova.StatusDaProva = StatusDaProva.Impresso;

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

                            return RedirectToAction("Index", "Provas");
                        }
                    }
                }
                else
                {
                    TempData["MensagemErro"] = "Professor não encontrado para esta prova ou o email do professor está vazio.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Ocorreu um erro ao atualizar o status da prova." });
            }
        }
    }
}
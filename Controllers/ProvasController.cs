using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Models;
using ProvaPortal.Repository.Interface;
using System.Drawing.Text;
using System.Text.RegularExpressions;

public class ProvasController : Controller
{
    private readonly IProvaRepository _provaRepository;
    private readonly ISessao _sessao;

    public ProvasController(IProvaRepository provaRepository, ISessao sessao)
    {
        _provaRepository = provaRepository;
        _sessao = sessao;
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

            string usuarioLogin = _sessao.BuscarUsuarioLoginNaSessao();
            if (string.IsNullOrEmpty(usuarioLogin))
            {

                return RedirectToAction("EnviarProva");
            }

            string nomeArquivo = $"{usuarioLogin}_{Guid.NewGuid()}__{numeroCopias}_Copias_.pdf";
            string caminhoArquivo = Path.Combine("ArquivosProva", nomeArquivo); // Especifique o caminho onde deseja salvar o arquivo.

            using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                arquivo.CopyTo(fileStream);
            }

            var prova = new ProvaModel
            {
                NumeroCopias = numeroCopias,
                NomeArquivo = nomeArquivo,
                DataEnvio = DateTime.Now,
            };

            _provaRepository.AdicionarProva(prova);

            // Redirecione para a ação Get EnviarProva, passando o modelo.
            return RedirectToAction("EnviarProva");
        }

        // Se não foi enviado um arquivo, simplesmente volte para a página de envio de prova.
        return RedirectToAction("EnviarProva");
    }



}

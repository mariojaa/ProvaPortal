using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

public class ProvasController : Controller
{
    private readonly IProvaRepository _provaRepository;

    public ProvasController(IProvaRepository provaRepository)
    {
        _provaRepository = provaRepository;
    }

    public async Task<IActionResult> Index()
    {
        var provas = await _provaRepository.GetProvasAsync();
        return View(provas);
    }

    public IActionResult EnviarProva()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EnviarProva(IFormFile arquivoProva)
    {
        if (arquivoProva != null && arquivoProva.Length > 0)
        {
            var nomeArquivo = Guid.NewGuid().ToString() + ".pdf";
            var caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", nomeArquivo);

            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                await arquivoProva.CopyToAsync(stream);
            }

            var prova = new ProvaModel
            {
                Nome = arquivoProva.FileName,
                CaminhoArquivo = nomeArquivo,
                DataEnvio = DateTime.Now
            };

            await _provaRepository.AddProvaAsync(prova);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var prova = await _provaRepository.GetProvaByIdAsync(id);
        if (prova == null)
        {
            return NotFound();
        }

        return View(prova);
    }
}

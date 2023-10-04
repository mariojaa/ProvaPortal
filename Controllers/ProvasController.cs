using Microsoft.AspNetCore.Mvc;

public class ProvasController : Controller
{
    private readonly IProvaRepository _provaRepository;

    public ProvasController(IProvaRepository provaRepository)
    {
        _provaRepository = provaRepository;
    }

    [HttpGet]
    public IActionResult EnviarProva()
    {
        // Verifique o último acesso aqui e armazene-o em uma variável, se necessário.
        DateTime ultimoAcesso = DateTime.Now; // Exemplo de obtenção da data/hora de acesso.

        // Verifique se um arquivo já foi enviado e passe o modelo para a view.
        var prova = _provaRepository.ObterTodasProvas().FirstOrDefault();

        return View(prova);
    }

    [HttpPost]
    public IActionResult EnviarProva(IFormFile arquivo, int nrCopias)
    {
        if (arquivo != null && arquivo.Length > 0)
        {
            
            // Salve o arquivo e as informações necessárias no repositório.
            string nomeArquivo = arquivo.FileName + $"{Guid.NewGuid()}__{nrCopias}_Copias_.pdf"; // Gere um nome de arquivo único.
            string caminhoArquivo = Path.Combine("ArquivosProva", nomeArquivo); // Especifique o caminho onde deseja salvar o arquivo.

            using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                arquivo.CopyTo(fileStream);
            }

            var prova = new ProvaModel
            {
                NrCopias = nrCopias,
                NomeArquivo = nomeArquivo,
                DataEnvio = DateTime.Now
            };

            _provaRepository.AdicionarProva(prova);

            // Redirecione para a ação Get EnviarProva, passando o modelo.
            return RedirectToAction("EnviarProva");
        }

        // Se não foi enviado um arquivo, simplesmente volte para a página de envio de prova.
        return RedirectToAction("EnviarProva");
    }
}

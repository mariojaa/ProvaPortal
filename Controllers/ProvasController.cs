﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProvaPortal.Data;
using ProvaPortal.Repository.Interface;

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
            if(string.IsNullOrEmpty(dadosSessaoProfessor))
            {
                return RedirectToAction("EnviarProva");
            }

            string nomeArquivo = $"{dadosSessaoProfessor}_{arquivo.FileName}_{numeroCopias}_Copias_.pdf";
            string caminhoArquivo = Path.Combine("ArquivosProva", nomeArquivo);

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
            _context.Provas.Add(prova);
            _context.SaveChanges();

            return RedirectToAction("EnviarProva");
        }

        return RedirectToAction("EnviarProva");
    }
}

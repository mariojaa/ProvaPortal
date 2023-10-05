﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProvaPortal.Models;

namespace ProvaPortal.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            ProfessorModel usuario = JsonConvert.DeserializeObject<ProfessorModel>(sessaoUsuario);
            return View(usuario);
        }
    }
}

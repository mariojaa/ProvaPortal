﻿using Newtonsoft.Json;
using ProvaPortal.Models;
using ProvaPortal.Repository.Interface;

namespace ProvaPortal.Repository
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ProfessorModel BuscarSessaoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoUsuarioLogado");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            return JsonConvert.DeserializeObject<ProfessorModel>(sessaoUsuario);
        }

        public void CriarSessaoUsuario(ProfessorDTO ProfessorDTO)
        {
            string valor = JsonConvert.SerializeObject(ProfessorDTO);
            _httpContext.HttpContext.Session.SetString("sessaoUsuarioLogado", valor);
        }

        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoUsuarioLogado");
        }

        public string BuscarDadosDaSessaoParaNomearArquivo( int numeroCopias)
        {
            ProfessorModel dadosDoCursoDoProfessorNaSessao = BuscarSessaoUsuario();
            
            var prova = new ProvaModel
            {
                NumeroCopias = numeroCopias,
            };
            if (dadosDoCursoDoProfessorNaSessao != null)
            {
                return dadosDoCursoDoProfessorNaSessao.UsuarioLogin;
            }
            return null;
        }
        public string BuscarSessaoDoUsuarioParaEnviarEmail(string email)
        {
            ProfessorModel professorModel = BuscarSessaoUsuario();
            professorModel.Email = email;
            return email;
        }
    }
}

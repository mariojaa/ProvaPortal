﻿using Microsoft.AspNetCore.Mvc.Rendering;
using ProvaPortal.Filters;
using ProvaPortal.Helper;
using ProvaPortal.Models.Enum;
using ProvaPortal.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
//public string CursoProfessor { get; set; }
//public int PeriodoProfessor { get; set; }
//public string TurmaProfessor { get; set; }
//8b6baafeccd4dcfe46721e1b796cde1c5b3b3d47
namespace ProvaPortal.Models
{
    
    public class ProfessorModel  //Pessoa //Usuário
    {
        public int Id { get; set; }
        public int Matricula { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public Perfil Perfil { get; set; }
        public string SenhaProfessor { get; set; }
        public string UsuarioLogin { get; set; }

        public List<CursoModel> CursoModels { get; set; }
        public List<ProvaModel> ProvaModels { get; set; }

        [NotMapped]
        [Compare("SenhaProfessor", ErrorMessage = "Senhas não conferem! Verifique e tente novamente.")]
        public string ConfirmarSenhaProfessor { get; set; }

        public bool SenhaValida(string senha)
        {
            return SenhaProfessor == senha.GerarHash();
        }

        public void SetSenhaHash()
        {
            SenhaProfessor = SenhaProfessor.GerarHash();
        }
        //public void SetNovaSenha(string novaSenha)
        //{
        //    SenhaProfessor = novaSenha.GerarHash();
        //}
        //public string GerarNovaSenha()
        //{
        //    string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
        //    SenhaProfessor = novaSenha.GerarHash();
        //    return novaSenha;
        //}

    }
}

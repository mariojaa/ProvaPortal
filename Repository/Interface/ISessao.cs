﻿using ProvaPortal.Models;

namespace ProvaPortal.Repository.Interface
{
    public interface ISessao
    {
        void CriarSessaoUsuario(ProfessorModel professorModel);
        void RemoverSessaoUsuario();
        string BuscarDadosDaSessaoParaNomearArquivo();
        ProfessorModel BuscarSessaoUsuario();

    }
}

using Microsoft.EntityFrameworkCore;
using ProvaPortal.Repository.Interface;
using ProvaPortal.Data;
using ProvaPortal.Models;

public class ProfessorRepository : IProfessorRepository
{
    private readonly ProvaPortalContext _context;

    public ProfessorRepository(ProvaPortalContext context)
    {
        _context = context;
    }

    public List<ProfessorModel> GetAllProfessores()
    {
        return _context.Professores.ToList();
    }

    public ProfessorModel GetProfessorById(int id)
    {
        return _context.Professores.FirstOrDefault(p => p.Id == id);
    }

    public void AddProfessor(ProfessorModel professor)
    {
        _context.Professores.Add(professor);
        _context.SaveChanges();
    }

    public void UpdateProfessor(ProfessorModel professor)
    {
        _context.Entry(professor).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void DeleteProfessor(int id)
    {
        var professor = GetProfessorById(id);
        if (professor != null)
        {
            _context.Professores.Remove(professor);
            _context.SaveChanges();
        }
    }
    public ProfessorModel BuscarPorLogin(string login)
    {
        return _context.Professores.FirstOrDefault(x => x.UsuarioLogin.ToUpper() == login.ToUpper());
    }
    public ProfessorModel Atualizar(ProfessorModel professorModel, int id)
    {
        ProfessorModel professorPorId = GetProfessorById(id);
        if (professorPorId == null)
        {
            throw new Exception($"Professor com Id {id}, não encontrado na base de dados do sistema! Contate o suporte@ugb.edu.br");
        }
        professorPorId.Matricula = professorModel.Matricula;
        professorPorId.NomeCompleto = professorModel.NomeCompleto;
        professorPorId.Email = professorModel.Email;

        _context.Professores.Update(professorPorId);
        _context.SaveChanges();
        return professorPorId;
    }
    public ProfessorModel BuscarPorEmailELogin(string email)
    {
        return _context.Professores.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper());
    }
}

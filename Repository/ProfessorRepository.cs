using ProvaPortal.Data;
using ProvaPortal.Models;
using ProvaPortal.Repository.Interface;
using System.Data;

namespace ProvaPortal.Repository
{
    public class ProfessorRepository : IProfessorRepository
    {
        public readonly ProvaPortalContext _context;
        public ProfessorRepository(ProvaPortalContext provaDbContext)
        {
            _context = provaDbContext;
        }
        public ProfessorModel Adicionar(ProfessorModel professorModel)
        {
            _context.Professores.Add(professorModel);
            _context.SaveChanges();
            return professorModel;
        }

        public ProfessorModel Atualizar(ProfessorModel professorModel, int id)
        {
            ProfessorModel professorPorId = BuscarPorId(id);
            if (professorPorId == null)
            {
                throw new Exception($"Professor com Id {id}, não encontrado na base de dados do sistema! Contate o suporte@ugb.edu.br");
            }
            professorPorId.Matricula = professorModel.Matricula;
            professorPorId.NomeCompleto = professorModel.NomeCompleto;
            professorPorId.Email = professorModel.Email;

            _context.Professores.Update(professorPorId);
            _context.SaveChanges();
            return professorPorId; // testar caso não, trocar por professorModel
        }

        public ProfessorModel BuscarPorId(int id)
        {
            return _context.Professores.FirstOrDefault(x => x.Id == id);
        }

        public List<ProfessorModel> BuscarTodosProfessores()
        {
            return _context.Professores.ToList();
        }

        public bool Remover(int id)
        {
            ProfessorModel professorPorId = BuscarPorId(id);
            if(professorPorId == null)
            {
                throw new Exception($"Professor com Id {id}, não encontrado na base de dados do sistema! Contate o suporte@ugb.edu.br");
            }
            _context.Professores.Remove(professorPorId);
            _context.SaveChanges();
            return true;
        }

        public void Update(ProfessorModel obj)
        {
            if (!_context.Professores.Any(x => x.Id == obj.Id))
            {
                throw new KeyNotFoundException("Id não encontrado");
            }
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
        }
    }
}

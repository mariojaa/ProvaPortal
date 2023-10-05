using ProvaPortal.Models;

namespace ProvaPortal.Repository.Interface
{
    public interface IProfessorRepository
    {
        List<ProfessorModel> BuscarTodosProfessores();
        ProfessorModel BuscarPorId(int id);
        ProfessorModel BuscarPorLogin(string login);
        ProfessorModel Adicionar(ProfessorModel professorModel);
        ProfessorModel Atualizar(ProfessorModel ProfessorModel, int id);
        void Remover(int id);
        void Update(ProfessorModel professorModel);
    }
}
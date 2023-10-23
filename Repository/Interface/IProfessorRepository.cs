using ProvaPortal.Models;

namespace ProvaPortal.Repository.Interface
{
    public interface IProfessorRepository
    {
        List<ProfessorModel> GetAllProfessores();
        ProfessorModel GetProfessorById(int id);
        void AddProfessor(ProfessorModel professor);
        void UpdateProfessor(ProfessorModel professor);
        void DeleteProfessor(int id);
        ProfessorModel BuscarPorLogin(string login);
    }
}
using ProvaPortal.Models;

public interface IProvaRepository
{
    List<ProvaModel> ObterTodasProvas(int professorId);
    ProvaModel AdicionarProva(ProvaModel prova);
    ProvaModel BuscarProvaPorId(int id);
    //List<ProvaModel> ObterProvasEnviadas(int id);
    void DeleteProva(int id);
}

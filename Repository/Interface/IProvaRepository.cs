using ProvaPortal.Models;

public interface IProvaRepository
{
    List<ProvaModel> ObterTodasProvas(int professorId);
    ProvaModel AdicionarProva(ProvaModel prova);
    ProvaModel BuscarProvaPorId(int id);
}

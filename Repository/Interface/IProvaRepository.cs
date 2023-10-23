using ProvaPortal.Models;

public interface IProvaRepository
{
    List<ProvaModel> ObterTodasProvas(int professorId);
    ProvaModel AdicionarProva(ProvaModel prova);
    ProvaModel BuscarProvaPorId(int id);
    void DeleteProva(int id);
    List<ProvaModel> ObterTodasProvasAdministrador();
    public List<ProvaModel> ObterTodasProvasAdministradorComProfessores();
    void AtualizarProva(ProvaModel prova);
}

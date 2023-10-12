public interface IProvaRepository
{
    List<ProvaModel> ObterTodasProvas(int professorId);
    void AdicionarProva(ProvaModel prova);
    ProvaModel BuscarProvaPorId(int id);
}

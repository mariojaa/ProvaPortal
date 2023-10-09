public interface IProvaRepository
{
    List<ProvaModel> ObterTodasProvas();
    void AdicionarProva(ProvaModel prova);
    ProvaModel BuscarProvaPorId(int id);
}

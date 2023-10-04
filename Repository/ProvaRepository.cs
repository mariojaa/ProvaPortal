public class ProvaRepository : IProvaRepository
{
    private List<ProvaModel> _provas = new List<ProvaModel>();

    public List<ProvaModel> ObterTodasProvas()
    {
        return _provas;
    }

    public void AdicionarProva(ProvaModel prova)
    {
        _provas.Add(prova);
    }
}

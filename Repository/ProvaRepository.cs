using ProvaPortal.Data;

public class ProvaRepository : IProvaRepository
{
    private List<ProvaModel> _provas = new List<ProvaModel>();

    public List<ProvaModel> ObterTodasProvas(int professorId)
    {
        return _provas.Where(x => x.ProfessorId == professorId).ToList();
    }

    public void AdicionarProva(ProvaModel prova)
    {
        _provas.Add(prova);
    }

    public ProvaModel BuscarProvaPorId(int id)
    {
        return _provas.FirstOrDefault(x => x.Id == id);
    }
}

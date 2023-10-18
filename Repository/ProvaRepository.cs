using ProvaPortal.Data;
using ProvaPortal.Models;

public class ProvaRepository : IProvaRepository
{
    private readonly ProvaPortalContext _context;

    public ProvaRepository(ProvaPortalContext context)
    {
        _context = context;
    }

    public List<ProvaModel> ObterTodasProvas(int professorId)
    {
        return _context.Provas.Where(x => x.ProfessorId == professorId).ToList();
    }

    public ProvaModel AdicionarProva(ProvaModel prova)
    {
        _context.Provas.Add(prova);
        _context.SaveChanges();
        return prova;
        
    }

    public ProvaModel BuscarProvaPorId(int id)
    {
        return _context.Provas.FirstOrDefault(x => x.Id == id);
    }
}

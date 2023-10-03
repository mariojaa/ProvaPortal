using Microsoft.EntityFrameworkCore;
using ProvaPortal.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProvaRepository : IProvaRepository
{
    private readonly ProvaPortalContext _context;

    public ProvaRepository(ProvaPortalContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProvaModel>> GetProvasAsync()
    {
        return await _context.Provas.ToListAsync();
    }

    public async Task<ProvaModel> GetProvaByIdAsync(int id)
    {
        return await _context.Provas.FindAsync(id);
    }

    public async Task AddProvaAsync(ProvaModel prova)
    {
        _context.Provas.Add(prova);
        await _context.SaveChangesAsync();
    }
}

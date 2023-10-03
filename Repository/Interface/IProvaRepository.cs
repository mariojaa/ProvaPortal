using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProvaRepository
{
    Task<IEnumerable<ProvaModel>> GetProvasAsync();
    Task<ProvaModel> GetProvaByIdAsync(int id);
    Task AddProvaAsync(ProvaModel prova);
}

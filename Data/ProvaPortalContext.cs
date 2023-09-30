using Microsoft.EntityFrameworkCore;
using ProvaPortal.Models;

namespace ProvaPortal.Data
{
    public class ProvaPortalContext : DbContext
    {
        public ProvaPortalContext(DbContextOptions<ProvaPortalContext> options) : base (options)
        {
            
        }
        public DbSet<ProfessorModel> Professores { get; set; }
    }
}

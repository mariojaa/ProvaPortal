using Microsoft.EntityFrameworkCore;
using ProvaPortal.Data.Map;
using ProvaPortal.Models;

namespace ProvaPortal.Data
{
    public class ProvaPortalContext : DbContext
    {
        public ProvaPortalContext(DbContextOptions<ProvaPortalContext> options) : base (options)
        {
            
        }
        public DbSet<ProfessorModel> Professores { get; set; }
        public DbSet<ProvaModel> Provas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProvaMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
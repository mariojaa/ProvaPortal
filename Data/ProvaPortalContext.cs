using AutoMapper;
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
        public DbSet<CursoModel> Cursos { get; set; }
        public DbSet<PeriodoModel> Periodos { get; set; }
        public DbSet<DisciplinaModel> Disciplinas { get; set; }
        public DbSet<Log> Logs { get; set; }

        public class MappingConfig
        {
            public static MapperConfiguration RegisterMaps()
            {
                var mappingConfig = new MapperConfiguration(config =>
                {
                    config.AddProfile(new AutoMapperProfile());
                });
                return mappingConfig;
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProfessorMap());
            modelBuilder.ApplyConfiguration(new CursoMap());
            modelBuilder.ApplyConfiguration(new PeriodoMap());
            modelBuilder.ApplyConfiguration(new DisciplinaMap());
            modelBuilder.ApplyConfiguration(new ProvaMap());

            base.OnModelCreating(modelBuilder);
        }
       

    }
}
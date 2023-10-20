using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProvaPortal.Models;

namespace ProvaPortal.Data.Map
{
    public class CursoMap : IEntityTypeConfiguration<CursoModel>
    {
        public void Configure(EntityTypeBuilder<CursoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Professor)
                .WithMany(p => p.CursoModels)
                .HasForeignKey(x => x.ProfessorId);
        }
    }
}
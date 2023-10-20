using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProvaPortal.Models;

namespace ProvaPortal.Data.Map
{
    public class PeriodoMap : IEntityTypeConfiguration<PeriodoModel>
    {
        public void Configure(EntityTypeBuilder<PeriodoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Curso)
                .WithMany(c => c.PeriodoModels)
                .HasForeignKey(x => x.CursoModelId);
        }
    }
}
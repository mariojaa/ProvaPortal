using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProvaPortal.Models;

namespace ProvaPortal.Data.Map
{
    public class DisciplinaMap : IEntityTypeConfiguration<DisciplinaModel>
    {
        public void Configure(EntityTypeBuilder<DisciplinaModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Periodo)
                .WithMany(p => p.DisciplinaModels)
                .HasForeignKey(x => x.PeriodoModelId);
        }
    }
}
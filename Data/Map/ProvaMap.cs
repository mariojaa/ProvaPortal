using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ProvaPortal.Data.Map
{
    public class ProvaMap : IEntityTypeConfiguration<ProvaModel>
    {
        public void Configure(EntityTypeBuilder<ProvaModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Professor)
                   .WithMany(p => p.ProvaModels)
                   .HasForeignKey(x => x.ProfessorId);
        }
    }
}
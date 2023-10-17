using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ProvaPortal.Data.Map
{
    public class ProvaMap : IEntityTypeConfiguration<ProvaModel>
    {
        public void Configure(EntityTypeBuilder<ProvaModel> builder)
        {
            builder.HasKey(x => x.Id);

            // Corrija o relacionamento com ProfessorModel
            builder.HasOne(x => x.ProfessorModels) // Use a propriedade de navegação, não o ID
                   .WithMany(p => p.ProvaModels)    // Defina a propriedade de navegação em ProfessorModel
                   .HasForeignKey(x => x.ProfessorId);
        }
    }
}

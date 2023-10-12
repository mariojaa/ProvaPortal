using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ProvaPortal.Data.Map
{
    public class ProvaMap : IEntityTypeConfiguration<ProvaModel>
    {
        public void Configure(EntityTypeBuilder<ProvaModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ProfessorModels) // Propriedade de navegação correta
                   .WithMany() // Defina isto de acordo com o relacionamento
                   .HasForeignKey(x => x.ProfessorId); // Chave estrangeira
        }
    }
}

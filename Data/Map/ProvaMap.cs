using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ProvaPortal.Data.Map
{
    public class ProvaMap : IEntityTypeConfiguration<ProvaModel>
    {
        public void Configure(EntityTypeBuilder<ProvaModel> builder)
        {
            builder.HasKey(x => x.Id);
            //builder.Property(x => x.NomeCompleto).HasMaxLength(255).IsRequired();
            builder.HasOne(x => x.NomeArquivo);
        }
    }
}

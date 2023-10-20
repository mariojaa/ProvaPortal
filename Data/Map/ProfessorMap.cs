using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProvaPortal.Models;

namespace ProvaPortal.Data.Map
{
    public class ProfessorMap : IEntityTypeConfiguration<ProfessorModel>
    {
        public void Configure(EntityTypeBuilder<ProfessorModel> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
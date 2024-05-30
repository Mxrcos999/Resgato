using Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mapping;

public class ProfessorMap : IEntityTypeConfiguration<Professor>
{
    public void Configure(EntityTypeBuilder<Professor> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.Professor)
            .HasForeignKey<ApplicationUser>(x => x.ProfessorId);
    }
}

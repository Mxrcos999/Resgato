using Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mapping;

public class ApplicationUserMap : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasOne(x => x.Professor)
            .WithOne(x => x.User)
            .HasForeignKey<Professor>(x => x.ApplicationUserId);
    }
}

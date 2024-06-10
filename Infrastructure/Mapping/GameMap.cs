using Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mapping;

public class GameMap : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        //builder.HasOne(x => x.)
        //    .WithMany(x => x.Games)
        //    .HasForeignKey(x => x.Professor);
    }
}

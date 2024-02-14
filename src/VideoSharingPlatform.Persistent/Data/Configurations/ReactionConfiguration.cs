using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Persistent.Data.Configurations;

public sealed class ReactionConfiguration : IEntityTypeConfiguration<Reaction> {
    public void Configure(EntityTypeBuilder<Reaction> builder) {
        builder.HasKey(x => new { x.UserId, x.VideoId });
    }
}
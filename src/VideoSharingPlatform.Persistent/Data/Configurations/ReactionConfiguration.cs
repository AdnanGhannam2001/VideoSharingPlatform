using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Persistent.Data.Configurations;

public sealed class ReactionConfiguration : IEntityTypeConfiguration<Reaction> {
    public void Configure(EntityTypeBuilder<Reaction> builder) {
        builder.HasKey(x => new { x.UserId, x.VideoId });

        builder.HasOne(x => x.Video)
            .WithMany(x => x.Reactions)
            .HasForeignKey(x => x.VideoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Comment)
            .WithMany(x => x.Reactions)
            .HasForeignKey(x => x.CommentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
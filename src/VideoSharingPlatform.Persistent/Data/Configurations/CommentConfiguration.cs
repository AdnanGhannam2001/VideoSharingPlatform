using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Persistent.Data.Configurations;

public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment> {
    public void Configure(EntityTypeBuilder<Comment> builder) {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Content)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasOne(x => x.Video)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.VideoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Replies)
            .HasForeignKey(x => x.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
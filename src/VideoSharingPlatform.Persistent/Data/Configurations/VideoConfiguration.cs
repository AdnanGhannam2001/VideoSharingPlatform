using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Persistent.Data.Configurations;

public sealed class VideoConfiguration : IEntityTypeConfiguration<Video> {
    public void Configure(EntityTypeBuilder<Video> builder) {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Uri)
            .IsRequired();

        builder.Property(x => x.Thumbnail)
            .IsRequired();

        builder.Property(x => x.Hidden)
            .HasDefaultValue(false);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Persistent.Data.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification> {
    public void Configure(EntityTypeBuilder<Notification> builder) {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Content)
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(x => x.Read)
            .HasDefaultValue(false);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;
namespace VideoSharingPlatform.Persistent.Data.Configurations;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser> {
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Notifications)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Videos)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Reactions)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Comments)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Subscribers)
            .WithOne(x => x.Subscribee)
            .HasForeignKey(x => x.SubscribeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SubscribedTo)
            .WithOne(x => x.Subscriber)
            .HasForeignKey(x => x.SubscriberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
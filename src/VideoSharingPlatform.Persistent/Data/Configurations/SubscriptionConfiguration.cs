using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Persistent.Data.Configurations;

public sealed class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription> {
    public void Configure(EntityTypeBuilder<Subscription> builder) {
        builder.HasKey(x => new { x.SubscribeeId, x.SubscriberId });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD_CQRS_Sample.Infrastructure.Outbox;

internal sealed class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Type).IsRequired();

        builder.Property(m => m.Content).IsRequired();
    }
}

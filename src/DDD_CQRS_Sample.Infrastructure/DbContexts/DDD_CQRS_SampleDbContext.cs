using DDD_CQRS_Sample.Domain.Products;
using DDD_CQRS_Sample.Infrastructure.Outbox;
using DDD_CQRS_Sample.Infrastructure.Outbox.Settings;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Data;
using Shared.Entities;
using System.Data;

namespace DDD_CQRS_Sample.Infrastructure.DbContexts;

public class DDD_CQRS_SampleDbContext : DbContext, IUnitOfWork
{
    public DDD_CQRS_SampleDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DDD_CQRS_SampleDbContext).Assembly);

        var auditableEntities = modelBuilder.Model.GetEntityTypes().Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)).ToList();
        foreach (var item in auditableEntities)
        {
            modelBuilder.Entity(item.ClrType)
                .Property<string>(AuditConstants.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder
                .Entity(item.ClrType)
                .Property<DateTime>(AuditConstants.CreatedOn)
                .IsRequired();

            modelBuilder
                .Entity(item.ClrType)
                .Property<string?>(AuditConstants.UpdatedBy)
                .HasMaxLength(50);

            modelBuilder
                .Entity(item.ClrType)
                .Property<DateTime?>(AuditConstants.UpdatedOn);
        }

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditFields();
        AddDomainEventsAsOutboxMessages();
        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    private void SetAuditFields()
    {
        var now = DateTime.Now;

        var auditableEntities = ChangeTracker.Entries().Where(m => m.Entity is IAuditable).ToList();
        foreach (var entry in auditableEntities)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(AuditConstants.CreatedBy).CurrentValue = "";
                entry.Property(AuditConstants.CreatedOn).CurrentValue = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(AuditConstants.UpdatedBy).CurrentValue = "";
                entry.Property(AuditConstants.UpdatedOn).CurrentValue = now;
            }
        }
    }

    private void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker.Entries<BaseEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    IReadOnlyList<IDomainEvent> domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(domainEvent => new OutboxMessage(
                    DateTime.Now,
                    domainEvent.GetType().Name,
                   JsonConvert.SerializeObject(domainEvent, OutboxSerilizerSettings.JsonSerializerSettings)))//do not use System.Text.Json
                .ToList();

        AddRange(outboxMessages);
    }
}
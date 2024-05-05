using DDD_CQRS_Sample.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Shared.Audit;
using Shared.DbContexts;

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
                .Property<DateTime>(AuditConstants.CreatedDate)
                .IsRequired();

            modelBuilder
                .Entity(item.ClrType)
                .Property<string?>(AuditConstants.UpdatedBy)
                .HasMaxLength(50);

            modelBuilder
                .Entity(item.ClrType)
                .Property<DateTime?>(AuditConstants.UpdatedDate);
        }

        base.OnModelCreating(modelBuilder);
    }
}
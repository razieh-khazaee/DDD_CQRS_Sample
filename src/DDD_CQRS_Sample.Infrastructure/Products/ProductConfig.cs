using DDD_CQRS_Sample.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD_CQRS_Sample.Infrastructure.Products;

internal class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(m => m.Id);

        builder
            .Property(m => m.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
           .Property(m => m.Brand)
           .HasMaxLength(100)
           .IsRequired();

        builder
            .Property(m => m.Description)
            .HasMaxLength(1000);

        builder.OwnsMany(m => m.ExtraInfos).ToJson();

        builder.HasIndex(m => new { m.Name, m.Brand }).IsUnique();
    }
}
using KaspelTestTask.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KaspelTestTask.Persistence.EntityTypeConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(ord => ord.IsSaved).IsRequired();
        builder.Property(ord => ord.OrderDate).IsRequired();
    }
}


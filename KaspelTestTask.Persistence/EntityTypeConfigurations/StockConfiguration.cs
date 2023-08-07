using KaspelTestTask.Core.Domain;
using KaspelTestTask.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KaspelTestTask.Persistence.EntityTypeConfigurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(stock => stock.BookId);
    }
}


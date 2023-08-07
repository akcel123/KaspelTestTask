using KaspelTestTask.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KaspelTestTask.Persistence.EntityTypeConfigurations;

public class BookConfiguration: IEntityTypeConfiguration<Book>
{


    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(book => book.Title).HasMaxLength(128).IsRequired();
        builder.Property(book => book.Author).HasMaxLength(128).IsRequired();
        builder.Property(book => book.Price).IsRequired();
        builder.Property(book => book.ReleaseDate).IsRequired();
        //builder.HasMany(book => book.OrderItems);
    }
}


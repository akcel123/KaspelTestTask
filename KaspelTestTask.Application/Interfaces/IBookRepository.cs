using KaspelTestTask.Application.Classes;
using KaspelTestTask.Core.Domain;

namespace KaspelTestTask.Application.Interfaces;

public interface IBookRepository
{
    public Task<IEnumerable<BookInformation>> GetAllBooksAsync();
    public Task<BookInformation?> GetBookByIdAsync(Guid id);
    public Task AddBookAsync(Book book, int quantity);
    public Task DeleteBookAsync(Guid id);

    public Task<IEnumerable<BookInformation>> GetBooksByFilterAsync(string? name, DateTime? releaseDate);

}


using AutoMapper;
using KaspelTestTask.Application.Classes;
using KaspelTestTask.Application.Exceptions;
using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Core.Domain;
using KaspelTestTask.Domain;
using Microsoft.EntityFrameworkCore;

namespace KaspelTestTask.Persistence.Repositories;

public class BookRepository: IBookRepository
{
    readonly IKaspelTestTaskDbContext _dbContext;
    readonly IMapper _mapper;

    public BookRepository(IKaspelTestTaskDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task AddBookAsync(Book book, int quantity)
    {
        var stock = new Stock() { Quantity = quantity, BookId = book.Id, Book = book };
        await _dbContext.Stock.AddAsync(stock);
        await _dbContext.SaveChangesAsync();
    }


    public async Task DeleteBookAsync(Guid id)
    {
        var entity = await _dbContext.Books.Where(b => b.Id == id).FirstOrDefaultAsync() ?? throw new ContentNotFoundException();
        _dbContext.Books.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<BookInformation>> GetAllBooksAsync()
    {
        List<BookInformation> booksInformation = new();
        var stocks = await _dbContext.Stock.Include(stock => stock.Book).Include(stock => stock.Book.OrderItems).ToListAsync();
        foreach (var stock in stocks)
        {
            var bookInformation = _mapper.Map<BookInformation>(stock);
            booksInformation.Add(bookInformation);
        }
        return booksInformation;
    }

    public async Task<BookInformation?> GetBookByIdAsync(Guid id)
    {
        var stock = await _dbContext.Stock
            .Include(stock => stock.Book)
            .Include(stock => stock.Book.OrderItems).Where(stock => stock.Book.Id == id).FirstOrDefaultAsync();
        if (stock == null) return null;
        var bookInformation = _mapper.Map<BookInformation>(stock);
        return bookInformation;
    }

    public async Task<IEnumerable<BookInformation>> GetBooksByFilterAsync(string? name, DateTime? releaseDate)
    {
        var query = _dbContext.Stock.Include(stock => stock.Book).AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(stock => stock.Book.Title.ToLower().Contains(name.ToLower()));
        }
        else if (releaseDate.HasValue)
        {
            query = query.Where(stock => stock.Book.ReleaseDate.Date == DateTime.SpecifyKind(releaseDate.Value.Date, DateTimeKind.Utc));
        }

        var stocks = await query.ToListAsync();

        List<BookInformation> booksInformation = new();
        foreach (var stock in stocks)
        {
            var bookInformation = _mapper.Map<BookInformation>(stock);
            booksInformation.Add(bookInformation);
        }
        return booksInformation;

    }
}


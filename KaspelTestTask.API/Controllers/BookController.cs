using AutoMapper;
using KaspelTestTask.API.Models.Book;
using KaspelTestTask.Application.Classes;
using KaspelTestTask.Application.Exceptions;
using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KaspelTestTask.API.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
[Produces("application/json")]
public class BookController : Controller
{
    private readonly ILogger<BookController> _logger;
    private readonly IBookRepository _repository;
    
    private readonly IMapper _mapper;

    public BookController(IBookRepository repository, ILogger<BookController> logger, IMapper mapper)
        => (_repository, _logger, _mapper) = (repository, logger, mapper);

    /// <summary>
    /// Get add books with query (optional)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="releaseDate"></param>
    /// <returns>list of GetBookDto</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /GetBooks
    ///     
    /// With query:
    ///     GET /GetBooks?name=foo&releaseDate=02%2F01%2F1000
    ///
    /// </remarks>
    /// <response code="200">Returns all books (if DB has no books, return embty array)</response>
    [HttpGet]
    [Route("GetBooks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetBookDto>>> GetAllBooksAsync([FromQuery] string? name, [FromQuery] DateTime? releaseDate)
    {
        IEnumerable<BookInformation> booksInformation;
        List<GetBookDto> getBookDtos = new();
        if (!string.IsNullOrEmpty(name) || releaseDate.HasValue)
            booksInformation = await _repository.GetBooksByFilterAsync(name, releaseDate);
        else
            booksInformation = await _repository.GetAllBooksAsync();

        foreach (var bookInformation in booksInformation)
        {
            var getBookDto = _mapper.Map<GetBookDto>(bookInformation);
            
            getBookDtos.Add(getBookDto);
        }

        return Ok(getBookDtos);
    }

    /// <summary>
    /// Get Book by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>return GetBookDto</returns>
    /// <response code="200">Book found</response>
    /// <response code="204">Book not found</response>
    [HttpGet]
    [Route("GetBook/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<GetBookDto>> GetBookByIdAsync(Guid id)
    {
        _logger.LogDebug($"Получение всей информации о книге, id: {id}");

        var bookInformation = await _repository.GetBookByIdAsync(id);

        if (bookInformation == null)
            throw new ContentNotFoundException();

        var getBookDto = _mapper.Map<GetBookDto>(bookInformation);
        return Ok(getBookDto);
    }

    /// <summary>
    /// Add New Book
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /AddBook
    ///     {
    ///         "title": "Bar",
    ///         "author": "Baz",
    ///         "releaseDate": "2023-08-06T14:25:36.745Z",
    ///         "price": 1,
    ///         "quantity": 1
    ///     }
    ///
    /// </remarks>
    /// <param name="dto"></param>
    /// <returns>return id added book</returns>
    /// <response code="201">If book was created</response>
    /// <response code="400">If Body is incorrect</response>
    [HttpPost]
    [Route("AddBook")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid?>> AddBookAsync(AddBookDto dto)
    {
        if (dto.Price <= 0 || dto.Quantity <= 0)
            throw new DtoIsNotValidException("Цена и количество книг должны быть больше 0");

        _logger.LogDebug($"Добавление книги в каталог");
        var book = new Book()
        {
            Author = dto.Author,
            Id = Guid.NewGuid(),
            Price = dto.Price,
            ReleaseDate = dto.ReleaseDate,
            Title = dto.Title,
        };

        await _repository.AddBookAsync(book, dto.Quantity);
        var addedBook = await _repository.GetBookByIdAsync(book.Id);
        return Created("AddBook", addedBook?.Id);

    }

    /// <summary>
    /// Delete Book With Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /DeleteBook/ec0b3819-4a36-4a90-8ee3-33622ddd8567
    ///
    /// </remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">If book was deleted</response>
    /// <response code="204">if book with Id not found</response>
    [HttpDelete]
    [Route("DeleteBook/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task DeleteBookAsync(Guid id)
    {
        await _repository.DeleteBookAsync(id);
    }
}


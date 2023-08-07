using AutoMapper;
using KaspelTestTask.API.Models.Order;
using KaspelTestTask.Application.Classes;
using KaspelTestTask.Application.Exceptions;
using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Core.Domain;
using Microsoft.AspNetCore.Mvc;


namespace KaspelTestTask.API.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
public class OrderController : Controller
{
    private readonly ILogger<BookController> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IMapper _mapper;

    public OrderController(ILogger<BookController> logger, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IStockRepository stockRepository, IMapper mapper)
        =>  (_logger, _orderRepository, _orderItemRepository, _stockRepository, _mapper) =
            (logger, orderRepository, orderItemRepository, stockRepository, mapper);

    /// <summary>
    /// Get all orders with query (optional)
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /GetOrders
    ///     
    /// With query:
    ///     GET /GetOrders?id=257b65a8-ac81-4078-a960-7bb9ec0bd850orderDate=02%2F01%2F1000
    ///
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="orderDate"></param>
    /// <returns>list of GetOrdersDto</returns>
    /// <response code="200">Returns all books (if DB has no books, return embty array)</response>
    [HttpGet]
    [Route("GetOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetOrdersDto>>> GetAllOrders([FromQuery] Guid? id, [FromQuery] DateTime? orderDate)
    {
        List<GetOrdersDto> getOrdersDtos = new();
        IEnumerable<OrderInformation> ordersInformation;
        _logger.LogTrace("Запрос на получение всех заказов");

        if (id.HasValue || orderDate.HasValue)
            ordersInformation = await _orderRepository.GetOrdersByFilterAsync(id, orderDate);
        else
            ordersInformation = await _orderRepository.GetAllOrdersAsync();

        foreach (var orderInformation in ordersInformation)
            getOrdersDtos.Add(_mapper.Map<GetOrdersDto>(orderInformation));

        return Ok(getOrdersDtos);
    }

    /// <summary>
    /// Create Order
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /CreateOrder
    ///     {
    ///         "books": [
    ///             {
    ///                 "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///                 "quantity": 3
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <param name="orderDto"></param>
    /// <returns>created order Id</returns>
    /// <response code="201">If order was created</response>
    /// <response code="400">If Body is incorrect</response>
    [HttpPost]
    [Route("CreateOrder")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateOrder(AddBooksToOrderDto orderDto)
    {

        _logger.LogTrace("Создание нового заказа");

        foreach (var book in orderDto.Books)
        {
            if (book.Quantity <= 0) throw new DtoIsNotValidException("Каким образом вы заказываете 0 книг?????");
            var booksInStock = await _stockRepository.GetNumberOfBookByIdAsync(book.BookId);
            if (booksInStock < book.Quantity)
                throw new FewBooksInStockException($"На складе осталось {book.Quantity} книг, а запрашивается {booksInStock}");
        } 

        var order = new Order()
        {
            Id = Guid.NewGuid(),
            IsSaved = false,
            OrderDate = DateTime.Now
        };

        await _orderRepository.AddOrderAsync(order);

        foreach (var book in orderDto.Books)
        {
            var orderItem = new OrderItem()
            {
                Id = Guid.NewGuid(),
                BookId = book.BookId,
                OrderId = order.Id,
                Quantity = book.Quantity
            };
            await _orderItemRepository.RegisterOrderItemAsync(orderItem);

            await _stockRepository.DecreaserNumberOfBookByIdAsync(book.BookId, book.Quantity);

        }

        return Created("CreateOrder", order.Id);
    }

    /// <summary>
    /// Add books to order
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /AddBook/b1d7d16e-2dca-444e-a07b-966a1a5bc09c
    ///     {
    ///         "books": [
    ///             {
    ///                 "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///                 "quantity": 1
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <response code="200">If books was added</response>
    /// <response code="204">If order with Id not found</response>
    /// <response code="400">If Body is incorrect</response>
    [HttpPost]
    [Route("AddBooks/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> AddBooksToOrder(Guid id,[FromBody] AddBooksToOrderDto dto)
    {
        _logger.LogTrace("Запрос на добавление книг в заказ");

        var order = await _orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NoContent();
        if (order.IsSaved)
            return BadRequest("Вы не можете добавить книги в сохраненную сделку");

        foreach (var book in dto.Books)
        {
            if (book.Quantity <= 0)
                throw new DtoIsNotValidException("Количество книг должны быть больше 0");

            var booksInStock = await _stockRepository.GetNumberOfBookByIdAsync(book.BookId);

            if (booksInStock < book.Quantity)
                throw new FewBooksInStockException($"На складе осталось {book.Quantity} книг, а запрашивается {booksInStock}");
        }

        foreach (var book in dto.Books)
        {
            await _orderItemRepository.UpdateOrderItemAsync(book.BookId, book.Quantity);
            await _stockRepository.DecreaserNumberOfBookByIdAsync(book.BookId, book.Quantity);
        }

        return Ok();
    }

    /// <summary>
    /// Save Order
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /SaveOrder/b1d7d16e-2dca-444e-a07b-966a1a5bc09c
    ///
    /// </remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">If order was saved</response>
    /// <response code="204">If order with Id not found</response>
    [HttpPatch]
    [Route("SaveOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SaveOrderById(Guid id)
    {
        _logger.LogTrace($"Запрос на сохранене заказа, id: {id}");
        await _orderRepository.SaveOrderByIdAsync(id);
        return Ok();
    }

    /// <summary>
    /// Delete Order
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /DeleteOrder/b1d7d16e-2dca-444e-a07b-966a1a5bc09c
    ///
    /// </remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">If order was deleted</response>
    /// <response code="204">If order with Id not found</response>
    [HttpDelete]
    [Route("DeleteOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteOrderByIdAsync(Guid id)
    {
        _logger.LogTrace($"Запрос на удаление заказа, id: {id}");
        await _orderRepository.DeleteOrderAsync(id);
        return Ok();
    }


}


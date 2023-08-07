using AutoMapper;
using KaspelTestTask.Application.Common.Mappings;

namespace KaspelTestTask.API.Models.Book;

public class AddBookDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }


}


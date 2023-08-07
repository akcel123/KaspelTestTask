using AutoMapper;
using KaspelTestTask.Application.Common.Mappings;
using KaspelTestTask.Domain;

namespace KaspelTestTask.Application.Classes;

public class BookInformation: IMapWith<Stock>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public List<Guid> Orders { get; set; } = new List<Guid>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Stock, BookInformation>()
            .ForMember(binf => binf.Id, opt => opt.MapFrom(stock => stock.Book.Id))
            .ForMember(binf => binf.Title, opt => opt.MapFrom(stock => stock.Book.Title))
            .ForMember(binf => binf.Author, opt => opt.MapFrom(stock => stock.Book.Author))
            .ForMember(binf => binf.ReleaseDate, opt => opt.MapFrom(stock => stock.Book.ReleaseDate))
            .ForMember(binf => binf.Price, opt => opt.MapFrom(stock => stock.Book.Price))
            .ForMember(binf => binf.Quantity, opt => opt.MapFrom(stock => stock.Quantity))
            .ForMember(binf => binf.Orders, opt => opt.MapFrom(stock => stock.Book.OrderItems.Select(ordIt => ordIt.OrderId)));
    }
}


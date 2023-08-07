using AutoMapper;
using KaspelTestTask.Application.Classes;
using KaspelTestTask.Application.Common.Mappings;

namespace KaspelTestTask.API.Models.Book;

public class GetBookDto: IMapWith<BookInformation>
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
        profile.CreateMap<BookInformation, GetBookDto>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(binf => binf.Id))
            .ForMember(dto => dto.Title, opt => opt.MapFrom(binf => binf.Title))
            .ForMember(dto => dto.Author, opt => opt.MapFrom(binf => binf.Author))
            .ForMember(dto => dto.ReleaseDate, opt => opt.MapFrom(binf => binf.ReleaseDate))
            .ForMember(dto => dto.Price, opt => opt.MapFrom(binf => binf.Price))
            .ForMember(dto => dto.Quantity, opt => opt.MapFrom(binf => binf.Quantity))
            .ForMember(dto => dto.Orders, opt => opt.MapFrom(binf => binf.Orders));
    }
}



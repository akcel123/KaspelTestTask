using AutoMapper;
using KaspelTestTask.API.Models.Book;
using KaspelTestTask.Application.Classes;
using KaspelTestTask.Application.Common.Mappings;

namespace KaspelTestTask.API.Models.Order;

public class GetOrdersDto: IMapWith<OrderInformation>
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsSaved { get; set; }

    public List<OrderItemInformation> OrderItems { get; set; } = new List<OrderItemInformation>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderInformation, GetOrdersDto>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(ord => ord.Id))
            .ForMember(dto => dto.OrderDate, opt => opt.MapFrom(ord => ord.OrderDate))
            .ForMember(dto => dto.IsSaved, opt => opt.MapFrom(ord => ord.IsSaved))
            .ForMember(dto => dto.OrderItems, opt => opt.MapFrom(ord => ord.OrderItems));
    }
}


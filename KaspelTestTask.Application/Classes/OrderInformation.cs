using AutoMapper;
using KaspelTestTask.Application.Common.Mappings;
using KaspelTestTask.Core.Domain;

namespace KaspelTestTask.Application.Classes;

public class OrderInformation: IMapWith<Order>
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsSaved { get; set; }

    public List<OrderItemInformation> OrderItems { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderInformation>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(ord => ord.Id))
            .ForMember(dto => dto.OrderDate, opt => opt.MapFrom(ord => ord.OrderDate))
            .ForMember(dto => dto.IsSaved, opt => opt.MapFrom(ord => ord.IsSaved))
            .ForMember(dto => dto.OrderItems, opt => opt.Ignore());
    }
}


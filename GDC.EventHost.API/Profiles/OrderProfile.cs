using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Order;

namespace GDC.EventHost.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                    dest => dest.OrderStatusValue,
                    opt => opt.MapFrom(src => src.OrderStatusId));

            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<OrderItemDto, OrderItem>();

            CreateMap<OrderForUpdateDto, Order>();

            CreateMap<Order, OrderForUpdateDto>();

            CreateMap<OrderForPatchDto, Order>();

            CreateMap<Order, OrderForPatchDto>();

            CreateMap<OrderItemForUpdateDto, OrderItem>();

            CreateMap<OrderItem, OrderItemForUpdateDto>();
        }
    }
}
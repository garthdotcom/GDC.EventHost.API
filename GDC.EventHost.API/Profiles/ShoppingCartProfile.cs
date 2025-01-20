using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.ShoppingCart;

namespace GDC.EventHost.API.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();

            CreateMap<ShoppingCartDto, ShoppingCart>();

            CreateMap<ShoppingCartItem, ShoppingCartItemDto>();

            CreateMap<ShoppingCartItemDto, ShoppingCartItem>();

            CreateMap<ShoppingCartForUpdateDto, ShoppingCart>();

            CreateMap<ShoppingCart, ShoppingCartForUpdateDto>();

            CreateMap<ShoppingCartItemForUpdateDto, ShoppingCartItem>();

            CreateMap<ShoppingCartItem, ShoppingCartItemForUpdateDto>();
        }
    }
}
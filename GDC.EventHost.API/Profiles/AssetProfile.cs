using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Asset;

namespace GDC.EventHost.API.Profiles
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            CreateMap<Asset, AssetDto>();

            CreateMap<Asset, AssetDetailDto>();

            CreateMap<AssetForUpdateDto, Asset>();

            CreateMap<Asset, AssetForUpdateDto>();
        }
    }
}

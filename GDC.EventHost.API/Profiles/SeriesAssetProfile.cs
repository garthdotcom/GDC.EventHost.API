using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.SeriesAsset;

namespace GDC.EventHost.API.Profiles
{
    public class SeriesAssetProfile : Profile
    {
        public SeriesAssetProfile()
        {
            CreateMap<SeriesAsset, SeriesAssetDto>()
                .ForMember(
                    dest => dest.AssetTypeValue,
                    opt => opt.MapFrom(src => src.AssetTypeId))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Asset.Description))
                .ForMember(
                    dest => dest.Uri,
                    opt => opt.MapFrom(src => src.Asset.Uri));

            CreateMap<SeriesAssetForUpdateDto, SeriesAsset>();

            CreateMap<SeriesAsset, SeriesAssetForUpdateDto>();
        }
    }
}
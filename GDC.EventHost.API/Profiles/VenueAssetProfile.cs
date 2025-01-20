using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.VenueAsset;

namespace GDC.EventHost.API.Profiles
{
    public class VenueAssetProfile : Profile
    {
        public VenueAssetProfile()
        {
            CreateMap<VenueAsset, VenueAssetDto>()
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

            CreateMap<VenueAssetForUpdateDto, VenueAsset>();

            CreateMap<VenueAsset, VenueAssetForUpdateDto>();
        }
    }
}

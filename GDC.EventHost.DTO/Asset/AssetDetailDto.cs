using GDC.EventHost.DTO.EventAsset;
using GDC.EventHost.DTO.PerformanceAsset;
using GDC.EventHost.DTO.SeriesAsset;
using GDC.EventHost.DTO.VenueAsset;

namespace GDC.EventHost.DTO.Asset
{
    public class AssetDetailDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required string Uri { get; set; }

        public List<SeriesAssetDto> SeriesAssets { get; set; } = [];

        public List<EventAssetDto> EventAssets { get; set; } = [];

        public List<PerformanceAssetDto> PerformanceAssets { get; set; } = [];

        public List<VenueAssetDto> VenueAssets { get; set; } = [];
    }
}

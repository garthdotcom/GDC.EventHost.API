using System;
using System.Collections.Generic;

namespace GDC.EventHost.DTO.Asset
{
    public class AssetDetailDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Uri { get; set; }

        public List<SeriesAssetDto> SeriesAssets { get; set; }
            = new List<SeriesAssetDto>();

        public List<EventSummaryAssetDto> EventSummaryAssets { get; set; }
            = new List<EventSummaryAssetDto>();

        public List<EventAssetDto> EventAssets { get; set; }
            = new List<EventAssetDto>();

        public List<VenueAssetDto> VenueAssets { get; set; }
            = new List<VenueAssetDto>();
    }
}

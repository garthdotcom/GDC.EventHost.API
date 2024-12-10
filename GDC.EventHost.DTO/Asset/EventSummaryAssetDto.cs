using System;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Asset
{
    public class EventSummaryAssetDto
    {
        public Guid Id { get; set; }

        public Guid EventSummaryId { get; set; }

        public Guid AssetId { get; set; }

        public AssetTypeEnum AssetTypeId { get; set; }

        public string AssetTypeValue { get; set; }

        public int OrdinalValue { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Uri { get; set; }
    }
}
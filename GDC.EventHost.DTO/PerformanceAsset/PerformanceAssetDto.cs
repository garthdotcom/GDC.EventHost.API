﻿using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.PerformanceAsset
{
    public class PerformanceAssetDto
    {
        public Guid Id { get; set; }

        public Guid PerformanceId { get; set; }

        public Guid AssetId { get; set; } 

        public AssetTypeEnum AssetTypeId { get; set; }

        public string AssetTypeValue { get; set; } = string.Empty;

        public int OrdinalValue { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;
    }
}
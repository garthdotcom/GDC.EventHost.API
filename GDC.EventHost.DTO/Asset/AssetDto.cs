using System;

namespace GDC.EventHost.DTO.Asset
{
    public class AssetDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Uri { get; set; }
    }
}

﻿namespace GDC.EventHost.API.ResourceParameters
{
    public class SeriesResourceParameters
    {
        public string? Title { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool IncludeDetail { get; set; } = false;
    }
}

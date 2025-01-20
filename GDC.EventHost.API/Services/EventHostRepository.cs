using GDC.EventHost.DAL.DbContexts;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Helpers;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.Shared.Performance;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static GDC.EventHost.Shared.Enums;

namespace GDC.EventHost.API.Services
{
    public class EventHostRepository : IEventHostRepository
    {
        private readonly EventHostContext _context;

        public EventHostRepository(EventHostContext context)
        {
            _context = context ??
                throw new NotImplementedException(nameof(context));
        }

        #region Asset

        public async Task<IEnumerable<Asset>> GetAssetsAsync()
        {
            var assets = await _context.Assets.ToListAsync();

            foreach (var asset in assets)
            {
                asset.SeriesAssets = await GetSeriesAssetsByAssetIdAsync(asset.Id);
                asset.PerformanceAssets = await GetPerformanceAssetsByAssetIdAsync(asset.Id);
                asset.EventAssets = await GetEventAssetsByAssetIdAsync(asset.Id);
                asset.VenueAssets = await GetAssetVenueAssetsByAssetIdAsync(asset.Id);
            }

            return assets;
        }

        public async Task<IEnumerable<Asset>> GetAssetsAsync(AssetResourceParameters resourceParms)
        {
            if (resourceParms == null)
            {
                throw new ArgumentNullException(nameof(resourceParms));
            }

            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var assetList = await GetAssetsAsync();

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return assetList;
            }

            // search
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                assetList = assetList.Where(s =>
                    s.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    s.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    s.Uri.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return assetList;
        }

        public async Task<Asset?> GetAssetByIdAsync(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            var asset = await _context.Assets
                .FirstOrDefaultAsync(s => s.Id == assetId);

            if (asset is not null)
            {
                asset.SeriesAssets = await GetSeriesAssetsByAssetIdAsync(asset.Id);
                asset.PerformanceAssets = await GetPerformanceAssetsByAssetIdAsync(asset.Id);
                asset.EventAssets = await GetEventAssetsByAssetIdAsync(asset.Id);
                asset.VenueAssets = await GetAssetVenueAssetsByAssetIdAsync(asset.Id);
            }

            return asset;
        }

        public async Task AddAssetAsync(Asset asset)
        {
            ArgumentNullException.ThrowIfNull(asset);

            await _context.Assets.AddAsync(asset);
        }

        public void DeleteAsset(Asset asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            _context.Assets.Remove(asset);
        }

        public async Task<bool> AssetExistsAsync(Guid assetId)
        {
            return await _context.Assets
                .AnyAsync(s => s.Id == assetId);
        }

        private async Task<List<SeriesAsset>> GetSeriesAssetsByAssetIdAsync(Guid assetId)
        {
            return await _context.SeriesAssets
                .Include(e => e.Series)
                .Where(e => e.AssetId == assetId)
                .ToListAsync();
        }

        private async Task<List<EventAsset>> GetEventAssetsByAssetIdAsync(Guid assetId)
        {
            return await _context.EventAssets
                .Include(e => e.Event)
                .Where(e => e.AssetId == assetId)
                .ToListAsync();
        }

        private async Task<List<PerformanceAsset>> GetPerformanceAssetsByAssetIdAsync(Guid assetId)
        {
            return await _context.PerformanceAssets
                .Include(e => e.Performance)
                .Where(e => e.AssetId == assetId)
                .ToListAsync();
        }

        private async Task<List<VenueAsset>> GetAssetVenueAssetsByAssetIdAsync(Guid assetId)
        {
            return await _context.VenueAssets
                .Include(e => e.Venue)
                .Where(e => e.AssetId == assetId)
                .ToListAsync();
        }

        #endregion

        #region Series

        public async Task<IEnumerable<Series>> GetSeriesAsync(bool includePast = false)
        {
            var seriesList = await GetAllSeriesAsync(includePast);

            foreach (var series in seriesList)
            {
                series.Events = await GetSeriesEventsAsync(series.Id, includePast);
                series.SeriesAssets = await GetSeriesAssetsAsync(series.Id);
            }

            return seriesList;
        }

        public async Task<IEnumerable<Series>> GetSeriesAsync(SeriesResourceParameters resourceParms,
            bool includePast = false)
        {
            if (resourceParms == null)
            {
                throw new ArgumentNullException(nameof(resourceParms));
            }

            var name = resourceParms.Title.FormatForSearch();
            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var seriesList = await GetSeriesAsync(includePast);

            if (string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return seriesList;
            }

            // filter on Name
            if (!string.IsNullOrWhiteSpace(name))
            {
                seriesList = seriesList
                    .Where(s => string.Equals(s.Title, name.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // search
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                seriesList = seriesList.Where(s =>
                    s.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    s.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return seriesList;
        }

        public async Task<Series?> GetSeriesByIdAsync(Guid seriesId, bool includePast = false)
        {
            if (seriesId == Guid.Empty)
            {
                return null;
            }

            var series = await _context.Series
                .Include(s => s.Status)
                .FirstOrDefaultAsync(s => s.Id == seriesId);

            if (series is not null)
            {
                series.Events = await GetSeriesEventsAsync(seriesId, includePast);
                series.SeriesAssets = await GetSeriesAssetsAsync(series.Id);
            }

            return series;
        }

        public async Task AddSeriesAsync(Series series)
        {
            ArgumentNullException.ThrowIfNull(series);

            await _context.Series.AddAsync(series);
        }

        public void DeleteSeries(Series series)
        {
            ArgumentNullException.ThrowIfNull(series);

            _context.Series.Remove(series);
        }

        public async Task<bool> SeriesExistsAsync(Guid seriesId)
        {
            return await _context.Series
                .AnyAsync(s => s.Id == seriesId);
        }

        public void SoftDeleteSeries(Series series)
        {
            if (series is not null)
            {
                series.StatusId = StatusEnum.Deleted;
            }
        }

        public async Task<List<SeriesAsset>> GetSeriesAssetsAsync(Guid seriesId)
        {
            if (seriesId == Guid.Empty)
            {
                return [];
            }

            return await _context.SeriesAssets
                .Include(e => e.AssetType)
                .Include(e => e.Series)
                .Include(e => e.Asset)
                .Where(e => e.SeriesId == seriesId)
                .OrderBy(e => e.AssetType.Name)
                    .ThenBy(e => e.OrdinalValue)
                .ToListAsync();
        }

        public async Task<List<SeriesAsset>> GetSeriesAssetsAsync(Guid seriesId,
            AssetResourceParameters resourceParms)
        {
            if (resourceParms.AssetTypeName is null)
            {
                return await GetSeriesAssetsAsync(seriesId);
            }

            if (Enum.TryParse<AssetTypeEnum>(resourceParms.AssetTypeName, true, out AssetTypeEnum parsedValue))
            {
                return await _context.SeriesAssets
                   .Include(e => e.AssetType)
                   .Include(e => e.Series)
                   .Include(e => e.Asset)
                   .Where(e => e.SeriesId == seriesId && e.AssetTypeId == parsedValue)
                   .OrderBy(e => e.OrdinalValue)
                   .ToListAsync();
            }

            return [];
        }

        public async Task<List<Event>> GetSeriesEventsAsync(Guid seriesId, bool includePast = false)
        {
            if (seriesId == Guid.Empty)
            {
                return [];
            }

            if (!includePast)
            {
                var currentEvents = await _context.Events
                    .Include(e => e.Status)
                    .Where(e => e.Status.Id != StatusEnum.Deleted &&
                     e.SeriesId == seriesId &&
                     (!e.EndDate.HasValue || e.EndDate.Value >= DateTime.Now))
                    .OrderBy(e => e.Title)
                    .ToListAsync();

                return currentEvents ?? [];
            }

            var allEvents = await _context.Events
                .Include(e => e.Status)
                .Where(e => e.Status.Id != StatusEnum.Deleted &&
                    e.SeriesId == seriesId)
                .OrderBy(e => e.Title)
                .ToListAsync();

            return allEvents ?? [];
        }

        public async Task<SeriesAsset?> GetSeriesAssetByIdAsync(Guid seriesAssetId)
        {
            if (seriesAssetId == Guid.Empty)
            {
                return null;
            }

            return await _context.SeriesAssets
                .Include(e => e.AssetType)
                .Include(e => e.Series)
                .Include(e => e.Asset)
                .FirstOrDefaultAsync(s => s.Id == seriesAssetId);
        }

        public async Task AddSeriesAssetAsync(SeriesAsset seriesAsset)
        {
            ArgumentNullException.ThrowIfNull(seriesAsset);

            if (await SeriesExistsAsync(seriesAsset.SeriesId) &&
                await AssetExistsAsync(seriesAsset.AssetId))
            {
                _context.SeriesAssets.Add(seriesAsset);
            }
        }

        public void DeleteSeriesAsset(SeriesAsset seriesAsset)
        {
            _context.SeriesAssets.Remove(seriesAsset);
        }

        public async Task<bool> SeriesAssetExistsAsync(Guid seriesId, AssetTypeEnum assetType)
        {
            return await _context.SeriesAssets
                .AnyAsync(s =>
                s.SeriesId == seriesId &&
                s.AssetTypeId == assetType);
        }

        private async Task<List<Series>> GetAllSeriesAsync(bool includePast = false)
        {
            if (!includePast)
            {
                // if there is no end date include it
                // if there is an end date this date must be future
                var currentSeriesList = await _context.Series
                    .Include(e => e.Status)
                    .Where(e => e.Status.Id != StatusEnum.Deleted &&
                     (!e.EndDate.HasValue || e.EndDate.Value >= DateTime.Now))
                    .OrderBy(e => e.Title)
                    .ToListAsync();

                return currentSeriesList ?? [];
            }

            var allSeriesList = _context.Series
                .Include(e => e.Status)
                .Where(e => e.Status.Id != StatusEnum.Deleted)
                .OrderBy(e => e.Title)
                .ToList();

            return allSeriesList ?? [];
        }

        #endregion

        #region Venue

        public async Task<IEnumerable<Venue>> GetVenuesAsync(bool includePast = false)
        {
            var venueList = await _context.Venues
                .Include(v => v.Status)
                .Where(v => v.Status.Id != StatusEnum.Deleted)
                .OrderBy(v => v.Name)
                .ToListAsync();

            foreach (var venue in venueList)
            {
                venue.Performances = await GetVenuePerformancesAsync(venue.Id, includePast);
                venue.VenueAssets = await GetVenueAssetsAsync(venue.Id);
                venue.SeatingPlans = await GetVenueSeatingPlansAsync(venue.Id);
            }

            return venueList;
        }

        public async Task<IEnumerable<Venue>> GetVenuesAsync(VenueResourceParameters resourceParms,
            bool includePast = false)
        {
            if (resourceParms == null)
            {
                throw new ArgumentNullException(nameof(resourceParms));
            }

            var name = resourceParms.Name.FormatForSearch();
            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var venueList = await GetVenuesAsync(includePast);

            if (string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return venueList;
            }

            // filter on Name
            if (!string.IsNullOrWhiteSpace(name))
            {
                venueList = venueList
                    .Where(s => string.Equals(s.Name, name.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // search
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                venueList = venueList.Where(s =>
                    s.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    s.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return venueList;
        }

        public async Task<Venue?> GetVenueByIdAsync(Guid venueId, bool includePast = false)
        {
            if (venueId == Guid.Empty)
            {
                return null;
            }

            var venue = await _context.Venues
                .Include(v => v.Status)
                .FirstOrDefaultAsync(v => v.Id == venueId);

            if (venue != null)
            {
                venue.Performances = await GetVenuePerformancesAsync(venue.Id, includePast);
                venue.VenueAssets = await GetVenueAssetsAsync(venue.Id);
                venue.SeatingPlans = await GetVenueSeatingPlansAsync(venue.Id);
            }

            return venue;
        }

        public async Task AddVenueAsync(Venue venue)
        {
            if (venue == null)
            {
                throw new ArgumentNullException(nameof(venue));
            }

            await _context.Venues.AddAsync(venue);
        }

        public void DeleteVenue(Venue venue)
        {
            ArgumentNullException.ThrowIfNull(venue);

            _context.Venues.Remove(venue);
        }

        public async Task<bool> VenueExistsAsync(Guid venueId)
        {
            return await _context.Venues
                .AnyAsync(v => v.Id == venueId);
        }

        public void SoftDeleteVenue(Venue venue)
        {
            if (venue != null)
            {
                venue.StatusId = StatusEnum.Deleted;
            }
        }

        public async Task<List<VenueAsset>> GetVenueAssetsAsync(Guid venueId)
        {
            return await _context.VenueAssets
                .Include(e => e.AssetType)
                .Include(e => e.Asset)
                .Include(e => e.Venue)
                .Where(e => e.VenueId == venueId)
                .OrderBy(e => e.AssetType.Name)
                .ThenBy(e => e.OrdinalValue)
                .ToListAsync();
        }

        public async Task<IEnumerable<VenueAsset>> GetVenueAssetsAsync(Guid venueId,
            AssetResourceParameters resourceParms)
        {
            if (resourceParms.AssetTypeName == null)
            {
                return await GetVenueAssetsAsync(venueId);
            }

            if (Enum.TryParse<AssetTypeEnum>(resourceParms.AssetTypeName, true, out AssetTypeEnum parsedValue))
            {
                return await _context.VenueAssets
                   .Include(e => e.AssetType)
                   .Include(e => e.Asset)
                   .Include(e => e.Venue)
                   .Where(e => e.VenueId == venueId && e.AssetTypeId == parsedValue)
                   .OrderBy(e => e.OrdinalValue)
                   .ToListAsync();
            }

            return [];
        }

        public async Task<List<Performance>> GetVenuePerformancesAsync(Guid venueId, bool includePast = false)
        {
            if (!includePast)
            {
                var currentPerformances = await _context.Performances
                    .Include(e => e.Status)
                    .Include(e => e.PerformanceType)
                    .Include(e => e.Venue)
                    .Include(e => e.Event)
                    .Include(e => e.SeatingPlan)
                    .Where(e =>
                        e.VenueId == venueId &&
                        e.Status.Id != StatusEnum.Deleted &&
                        e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .ToListAsync();

                return currentPerformances ?? [];
            }

            var allPerformances = await _context.Performances
                .Include(e => e.Status)
                .Include(e => e.PerformanceType)
                .Include(e => e.Venue)
                .Include(e => e.Event)
                .Include(e => e.SeatingPlan)
                .Where(e =>
                    e.VenueId == venueId &&
                    e.Status.Id != StatusEnum.Deleted)
                .OrderBy(e => e.Date)
                .ToListAsync();

            return allPerformances ?? [];
        }

        public async Task<List<SeatingPlan>> GetVenueSeatingPlansAsync(Guid venueId)
        {
            var venueSeatingPlans = await _context.SeatingPlans
                .Where(e =>
                    e.Status.Id != StatusEnum.Deleted &&
                    e.VenueId == venueId)
                .OrderBy(e => e.Name)
                .ToListAsync();

            return venueSeatingPlans ?? [];
        }

        public async Task<VenueAsset?> GetVenueAssetByIdAsync(Guid venueAssetId)
        {
            if (venueAssetId == Guid.Empty)
            {
                return null;
            }

            return await _context.VenueAssets
                .Include(e => e.AssetType)
                .Include(e => e.Asset)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.Id == venueAssetId);
        }

        public async Task AddVenueAssetAsync(VenueAsset venueAsset)
        {
            ArgumentNullException.ThrowIfNull(venueAsset);

            if (await VenueExistsAsync(venueAsset.VenueId) &&
                await AssetExistsAsync(venueAsset.AssetId))
            {
                _context.VenueAssets.Add(venueAsset);
            }
        }

        public void DeleteVenueAsset(VenueAsset venueAsset)
        {
            _context.VenueAssets.Remove(venueAsset);
        }

        public async Task<bool> VenueAssetExistsAsync(Guid venueId, AssetTypeEnum assetType)
        {
            return await _context.VenueAssets
                .AnyAsync(s =>
                s.VenueId == venueId &&
                s.AssetTypeId == assetType);
        }

        #endregion

        #region Event

        public async Task<IEnumerable<Event>> GetEventsAsync(bool includePast = false)
        {
            var events = await GetAllEventsAsync(includePast);

            foreach (var evt in events)
            {
                evt.Performances = await GetEventPerformancesAsync(evt.Id, includePast);
                evt.EventAssets = await GetEventAssetsAsync(evt.Id);
            }

            return events;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(EventResourceParameters resourceParms,
            bool includePast = false)
        {
            ArgumentNullException.ThrowIfNull(resourceParms);

            var title = resourceParms.Title.FormatForSearch();
            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var eventList = await GetEventsAsync(includePast);

            if (string.IsNullOrWhiteSpace(title) &&
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return eventList;
            }

            // filter
            if (!string.IsNullOrWhiteSpace(title))
            {
                eventList = eventList
                    .Where(s => string.Equals(s.Title, title.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // search
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();

                // 1. if searchQuery is a full series name, return those events that belong
                //    to that series

                var eventsForSeries = eventList
                    .Where(s => s.Series.Title == searchQuery);

                if (eventsForSeries.Count() > 0)
                {
                    return eventsForSeries;
                }

                var allPerformances = await GetAllPerformancesAsync();

                // 2. if searchQuery is a full venue name, return those events that have a
                //    future performance at that venue

                var performancesAtVenue = allPerformances
                    .Where(e => e.Venue.Name == searchQuery);

                if (performancesAtVenue.Count() > 0)
                {
                    return performancesAtVenue.Select(e => e.Event).Distinct();
                }

                // 3. if searchQuery is an abbreviated month name, return those events that
                //    have a performance taking place in the given month

                var performancesForMonth = allPerformances.Where(e =>
                    CultureInfo.CurrentCulture.DateTimeFormat
                    .GetAbbreviatedMonthName(e.Date.Month) == searchQuery);

                if (performancesForMonth.Count() > 0)
                {
                    return performancesForMonth.Select(e => e.Event).Distinct();
                }

                // 4. match on full or partial event title

                eventList = eventList.Where(e =>
                    e.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return eventList;
        }

        public async Task<Event?> GetEventByIdAsync(Guid eventId, bool includePast = false)
        {
            if (eventId == Guid.Empty)
            {
                return null;
            }

            var evt = await _context.Events
                .Include(e => e.Status)
                .Include(e => e.Series)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (evt != null)
            {
                evt.Performances = await GetEventPerformancesAsync(evt.Id, includePast);
                evt.EventAssets = await GetEventAssetsAsync(evt.Id);
            }

            return evt;
        }

        public async Task AddEventAsync(Event evt)
        {
            ArgumentNullException.ThrowIfNull(evt);

            await _context.Events.AddAsync(evt);
        }

        public void DeleteEvent(Event evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException(nameof(evt));
            }

            _context.Events.Remove(evt);
        }

        public async Task<bool> EventExistsAsync(Guid eventId)
        {
            return await _context.Events
                .AnyAsync(s => s.Id == eventId);
        }

        public void SoftDeleteEvent(Event evt)
        {
            if (evt != null)
            {
                evt.StatusId = StatusEnum.Deleted;
            }
        }

        public async Task<List<EventAsset>> GetEventAssetsAsync(Guid eventId)
        {
            return await _context.EventAssets
                .Include(e => e.AssetType)
                .Include(e => e.Event)
                .Include(e => e.Asset)
                .Where(e => e.EventId == eventId)
                .OrderBy(e => e.AssetType.Name)
                .ThenBy(e => e.OrdinalValue)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventAsset>> GetEventAssetsAsync(Guid eventId,
            AssetResourceParameters resourceParms)
        {
            if (resourceParms.AssetTypeName == null)
            {
                return await GetEventAssetsAsync(eventId);
            }

            if (Enum.TryParse<AssetTypeEnum>(resourceParms.AssetTypeName, true, out AssetTypeEnum parsedValue))
            {
                return await _context.EventAssets
                    .Include(e => e.AssetType)
                    .Include(e => e.Event)
                    .Include(e => e.Asset)
                    .Where(e => e.EventId == eventId && e.AssetTypeId == parsedValue)
                    .OrderBy(e => e.OrdinalValue)
                    .ToListAsync();
            }

            return [];
        }

        public async Task<List<Performance>> GetEventPerformancesAsync(Guid eventId, bool includePast = false)
        {
            if (!includePast)
            {
                var currentPerfomances = await _context.Performances
                    .Include(p => p.Status)
                    .Include(p => p.Event)
                    .Include(p => p.Venue)
                    .Where(p => p.EventId == eventId &&
                         p.Status.Id != StatusEnum.Deleted &&
                         p.Date >= DateTime.Now)
                    .OrderBy(p => p.Date)
                    .ToListAsync();

                return currentPerfomances ?? [];
            }

            var allPerfomances = await _context.Performances
                .Include(p => p.Status)
                .Include(p => p.Event)
                .Include(p => p.Venue)
                .Where(p => p.EventId == eventId &&
                    p.Status.Id != StatusEnum.Deleted)
                .OrderBy(p => p.Date)
                .ToListAsync();

            return allPerfomances ?? [];
        }

        public async Task<EventAsset?> GetEventAssetByIdAsync(Guid eventAssetId)
        {
            if (eventAssetId == Guid.Empty)
            {
                return null;
            }

            return await _context.EventAssets
                .Include(e => e.AssetType)
                .Include(e => e.Event)
                .Include(e => e.Asset)
                .FirstOrDefaultAsync(e => e.Id == eventAssetId);
        }

        public async Task AddEventAssetAsync(EventAsset eventAsset)
        {
            ArgumentNullException.ThrowIfNull(eventAsset);

            if (await EventExistsAsync(eventAsset.EventId) &&
                await AssetExistsAsync(eventAsset.AssetId))
            {
                _context.EventAssets.Add(eventAsset);
            }
        }

        public void DeleteEventAsset(EventAsset eventAsset)
        {
            _context.EventAssets
                .Remove(eventAsset);
        }

        public async Task<bool> EventAssetExistsAsync(Guid eventId, AssetTypeEnum assetType)
        {
            return await _context.EventAssets
                .AnyAsync(s =>
                s.EventId == eventId &&
                s.AssetTypeId == assetType);
        }

        private async Task<IEnumerable<Event>> GetAllEventsAsync(bool includePast = false)
        {
            if (!includePast)
            {
                // if there is no end date include it
                // if there is an end date this date must be future
                var currentEvents = await _context.Events
                    .Include(e => e.Status)
                    .Include(e => e.Series)
                    .Where(e => e.Status.Id != StatusEnum.Deleted &&
                     (!e.EndDate.HasValue || e.EndDate.Value >= DateTime.Now))
                    .OrderBy(e => e.Title)
                    .ToListAsync();

                return currentEvents ?? [];
            }

            var allEvents = await _context.Events
                .Include(e => e.Status)
                .Include(e => e.Series)
                .Where(e => e.Status.Id != StatusEnum.Deleted)
                .OrderBy(e => e.Title)
                .ToListAsync();

            return allEvents ?? [];
        }

        #endregion

        #region Performance

        public async Task<IEnumerable<Performance>> GetPerformancesAsync(bool includePast = false)
        {
            var performanceList = await GetAllPerformancesAsync(includePast);

            foreach (var performance in performanceList)
            {
                performance.Tickets = await GetPerformanceTicketsAsync(performance.Id);
                performance.PerformanceAssets = await GetPerformanceAssetsAsync(performance.Id);
            }

            return performanceList;
        }

        public async Task<IEnumerable<Performance>> GetPerformancesAsync(PerformanceResourceParameters resourceParms,
            bool includePast = false)
        {
            if (resourceParms == null)
            {
                throw new ArgumentNullException(nameof(resourceParms));
            }

            var title = resourceParms.Title.FormatForSearch();
            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var performanceList = await GetPerformancesAsync(includePast);

            if (string.IsNullOrWhiteSpace(title) &&
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return performanceList;
            }

            // filter
            if (!string.IsNullOrWhiteSpace(title))
            {
                performanceList = performanceList
                    .Where(p => string.Equals(p.Title, title.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // search
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();

                if (DateTime.TryParse(searchQuery, out DateTime searchDateTime))
                {
                    // search on date
                    return performanceList.Where(s =>
                            s.Date.Day == searchDateTime.Day &&
                            s.Date.Month == searchDateTime.Month &&
                            s.Date.Year == searchDateTime.Year);
                }

                // search on venue name
                return performanceList.Where(s =>
                    s.Venue != null &&
                    s.Venue.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return performanceList;
        }

        public async Task<Performance?> GetPerformanceByIdAsync(Guid performanceId)
        {
            if (performanceId == Guid.Empty)
            {
                return null;
            }

            var performance = await _context.Performances
                .Include(e => e.Status)
                .Include(e => e.PerformanceType)
                .Include(e => e.Venue)
                .Include(e => e.Event)
                .Include(e => e.SeatingPlan)
                .FirstOrDefaultAsync(e => e.Id == performanceId);

            if (performance != null)
            {
                performance.Tickets = await GetPerformanceTicketsAsync(performance.Id);
                performance.PerformanceAssets = await GetPerformanceAssetsAsync(performance.Id);
            }

            return performance;
        }

        public async Task AddPerformanceAsync(Performance performance)
        {
            ArgumentNullException.ThrowIfNull(performance);

            await _context.Performances
                .AddAsync(performance);
        }

        public void DeletePerformance(Performance performance)
        {
            ArgumentNullException.ThrowIfNull(performance);

            _context.Performances
                .Remove(performance);
        }

        public async Task<bool> PerformanceExistsAsync(Guid performanceId)
        {
            return await _context.Performances
                .AnyAsync(p => p.Id == performanceId);
        }

        public void SoftDeletePerformance(Performance performance)
        {
            if (performance != null)
            {
                performance.StatusId = StatusEnum.Deleted;
            }
        }

        public async Task<List<PerformanceAsset>> GetPerformanceAssetsAsync(Guid performanceId)
        {
            var performanceAssets = await _context.PerformanceAssets
                .Include(e => e.AssetType)
                .Include(e => e.Performance)
                .Include(e => e.Asset)
                .Where(e => e.PerformanceId == performanceId)
                .OrderBy(e => e.AssetType.Name)
                    .ThenBy(e => e.OrdinalValue)
                .ToListAsync();

            return performanceAssets ?? new List<PerformanceAsset>();
        }

        public async Task<IEnumerable<PerformanceAsset>> GetPerformanceAssetsAsync(Guid performanceId,
            AssetResourceParameters resourceParms)
        {
            if (resourceParms.AssetTypeName is null)
            {
                return await GetPerformanceAssetsAsync(performanceId);
            }

            if (Enum.TryParse<AssetTypeEnum>(resourceParms.AssetTypeName, true, out AssetTypeEnum parsedValue))
            {
                return await _context.PerformanceAssets
                    .Include(e => e.AssetType)
                    .Include(e => e.Performance)
                    .Include(e => e.Asset)
                    .Where(e => e.PerformanceId == performanceId && e.AssetTypeId == parsedValue)
                    .OrderBy(e => e.OrdinalValue)
                    .ToListAsync();
            }

            return [];
        }

        public async Task<List<Ticket>> GetPerformanceTicketsAsync(Guid performanceId)
        {
            var ticketList = await _context.Tickets
                .Include(e => e.TicketStatus)
                .Include(e => e.Performance)
                .Where(e => e.PerformanceId == performanceId)
                .OrderBy(e => e.Number)
                .ToListAsync();

            return ticketList ?? [];
        }

        public async Task<bool> PerformanceTicketsExistAsync(Guid performanceId)
        {
            return await _context.Tickets
                .AnyAsync(e => e.PerformanceId == performanceId);
        }

        public async Task<PerformanceAsset?> GetPerformanceAssetByIdAsync(Guid performanceAssetId)
        {
            if (performanceAssetId == Guid.Empty)
            {
                return null;
            }

            return await _context.PerformanceAssets
                .Include(e => e.AssetType)
                .Include(e => e.Performance)
                .Include(e => e.Asset)
                .FirstOrDefaultAsync(s => s.Id == performanceAssetId);
        }

        public async Task AddPerformanceAssetAsync(PerformanceAsset performanceAsset)
        {
            ArgumentNullException.ThrowIfNull(performanceAsset);

            var performance = await _context.Performances
                .FirstOrDefaultAsync(s => s.Id == performanceAsset.PerformanceId);

            if (performance is not null)
            {
                await _context.PerformanceAssets
                    .AddAsync(performanceAsset);
            }
        }

        public void DeletePerformanceAsset(PerformanceAsset performanceAsset)
        {
            _context.PerformanceAssets
                .Remove(performanceAsset);
        }

        public async Task<bool> PerformanceAssetExistsAsync(Guid performanceId, AssetTypeEnum assetType)
        {
            return await _context.PerformanceAssets.AnyAsync(s =>
                s.PerformanceId == performanceId &&
                s.AssetTypeId == assetType);
        }

        private async Task<IEnumerable<Performance>> GetAllPerformancesAsync(bool includePast = false)
        {
            if (!includePast)
            {
                var currentPerformances = await _context.Performances
                    .Include(e => e.Status)
                    .Include(e => e.PerformanceType)
                    .Include(e => e.Venue)
                    .Include(e => e.Event)
                    .Include(e => e.SeatingPlan)
                    .Where(e => e.Status.Id != StatusEnum.Deleted && e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .ToListAsync();

                return currentPerformances ?? [];
            }

            var allPerformances = _context.Performances
                .Include(e => e.Status)
                .Include(e => e.PerformanceType)
                .Include(e => e.Venue)
                .Include(e => e.Event)
                .Include(e => e.SeatingPlan)
                .Where(e => e.Status.Id != StatusEnum.Deleted)
                .OrderBy(e => e.Date)
                .ToList();

            return allPerformances ?? [];
        }

        #endregion

        #region PerformanceType

        public async Task<IEnumerable<PerformanceType>> GetPerformanceTypesAsync(bool includePast = false)
        {
            var allPerformanceTypes = await _context.PerformanceTypes
                .OrderBy(e => e.Name)
                .ToListAsync();

            foreach (var performanceType in allPerformanceTypes)
            {
                performanceType.Performances = await GetPerformanceTypePerformancesAsync(performanceType.Id, includePast);
            }

            return allPerformanceTypes;
        }

        public async Task<PerformanceType?> GetPerformanceTypeByIdAsync(Guid performanceTypeId, bool includePast = false)
        {
            var performanceType = await _context.PerformanceTypes
                .FirstOrDefaultAsync(e => e.Id == performanceTypeId);

            if (performanceType != null)
            {
                performanceType.Performances = await GetPerformanceTypePerformancesAsync(performanceType.Id, includePast);
            }

            return performanceType;
        }

        public async Task AddPerformanceTypeAsync(PerformanceType performanceType)
        {
            ArgumentNullException.ThrowIfNull(performanceType);

            await _context.PerformanceTypes.AddAsync(performanceType);
        }

        public async Task<bool> PerformanceTypeExistsAsync(Guid performanceTypeId)
        {
            return await _context.PerformanceTypes
                .AnyAsync(e => e.Id == performanceTypeId);
        }

        public async Task<List<Performance>> GetPerformanceTypePerformancesAsync(Guid performanceTypeId, bool includePast = false)
        {
            if (!includePast)
            {
                var currentPerformances = await _context.Performances
                    .Include(e => e.Status)
                    .Include(e => e.PerformanceType)
                    .Include(e => e.Venue)
                    .Include(e => e.Event)
                    .Where(e => e.PerformanceTypeId == performanceTypeId &&
                     e.Status.Id != StatusEnum.Deleted &&
                     e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .ToListAsync();

                return currentPerformances ?? [];
            }

            var allPerformances = await _context.Performances
                .Include(e => e.Status)
                .Include(e => e.PerformanceType)
                .Include(e => e.Venue)
                .Include(e => e.Event)
                .Where(e => e.PerformanceTypeId == performanceTypeId &&
                 e.Status.Id != StatusEnum.Deleted)
                .OrderBy(e => e.Date)
                .ToListAsync();

            return allPerformances ?? [];
        }

        #endregion

        #region SeatingPlan

        public async Task<IEnumerable<SeatingPlan>> GetSeatingPlansAsync(bool includePast = false)
        {
            var seatingPlanList = await _context.SeatingPlans
                .Include(e => e.Status)
                .Include(e => e.Venue)
                .Where(e => e.Status.Id != StatusEnum.Deleted)
                .OrderBy(e => e.Name)
                .ToListAsync();

            foreach (var seatingPlan in seatingPlanList)
            {
                seatingPlan.Performances = await GetSeatingPlanPerformancesAsync(seatingPlan.Id, includePast);
            }

            return seatingPlanList ?? [];
        }

        public async Task<IEnumerable<SeatingPlan>> GetSeatingPlansAsync(SeatingPlanResourceParameters resourceParms,
            bool includePast = false)
        {
            ArgumentNullException.ThrowIfNull(resourceParms);

            var name = resourceParms.Name.FormatForSearch();
            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var seatingPlanList = await GetSeatingPlansAsync(includePast);

            if (string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return seatingPlanList;
            }

            // filter on Name
            if (!string.IsNullOrWhiteSpace(name))
            {
                seatingPlanList = seatingPlanList
                    .Where(s => string.Equals(s.Name, name.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // search
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                seatingPlanList = seatingPlanList.Where(s =>
                    s.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    s.Venue.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return seatingPlanList;
        }

        public async Task<List<Performance>> GetSeatingPlanPerformancesAsync(Guid seatingPlanId, bool includePast = false)
        {
            if (!includePast)
            {
                var currentPerformances = await _context.Performances
                    .Include(e => e.Status)
                    .Include(e => e.PerformanceType)
                    .Include(e => e.Venue)
                    .Include(e => e.Event)
                    .Include(e => e.SeatingPlan)
                    .Where(e =>
                        e.SeatingPlanId == seatingPlanId &&
                        e.Status.Id != StatusEnum.Deleted &&
                        e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .ToListAsync();

                return currentPerformances ?? [];
            }

            var allPerformances = await _context.Performances
                .Include(e => e.Status)
                .Include(e => e.PerformanceType)
                .Include(e => e.Venue)
                .Include(e => e.Event)
                .Include(e => e.SeatingPlan)
                .Where(e =>
                    e.SeatingPlanId == seatingPlanId &&
                    e.Status.Id != StatusEnum.Deleted)
                .OrderBy(e => e.Date)
                .ToListAsync();

            return allPerformances ?? [];
        }

        public async Task<SeatingPlan?> GetSeatingPlanByIdAsync(Guid seatingPlanId, bool includePast = false)
        {
            var seatingPlan = await _context.SeatingPlans
                .Include(e => e.Status)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.Id == seatingPlanId);

            if (seatingPlan is null)
            {
                seatingPlan.Performances = await GetSeatingPlanPerformancesAsync(seatingPlanId, includePast);
            }

            return seatingPlan;
        }

        public async Task AddSeatingPlanAsync(SeatingPlan seatingPlan)
        {
            ArgumentNullException.ThrowIfNull(seatingPlan);

            await _context.SeatingPlans.AddAsync(seatingPlan);
        }


        public void DeleteSeatingPlan(SeatingPlan seatingPlan)
        {
            ArgumentNullException.ThrowIfNull(seatingPlan);

            _context.SeatingPlans.Remove(seatingPlan);
        }

        public async Task<bool> SeatingPlanExistsAsync(Guid seatingPlanId)
        {
            return await _context.SeatingPlans
                .AnyAsync(s => s.Id == seatingPlanId);
        }

        public void SoftDeleteSeatingPlan(SeatingPlan seatingPlan)
        {
            if (seatingPlan != null)
            {
                seatingPlan.StatusId = StatusEnum.Deleted;
            }
        }

        #endregion

        #region SeatPosition

        public async Task<IEnumerable<SeatPosition>> GetSeatPositionsAsync()
        {
            return await _context.SeatPositions
                .Include(s => s.SeatPositionType)
                .ToListAsync();
        }

        public async Task<SeatPosition?> GetSeatPositionByIdAsync(Guid seatPositionId)
        {
            return await _context.SeatPositions
                .Include(s => s.SeatPositionType)
                .FirstOrDefaultAsync(s => s.Id == seatPositionId);
        }

        public IEnumerable<SeatPosition> GetRootSeatPositions(Guid seatingPlanId)
        {
            var rootPositions = _context.SeatPositions
                .Include(e => e.SeatPositionType)
                .Where(e => e.ParentId == null && e.SeatingPlanId == seatingPlanId)
                .OrderBy(e => e.OrdinalValue)
                .ToList();

            return rootPositions ?? [];
        }

        public List<Seat> GetSeatsForParent(Guid seatPositionId)
        {
            var seats = _context.Seats
                .Include(s => s.SeatType)
                .Where(s => s.ParentId == seatPositionId)
                .OrderBy(s => s.OrdinalValue)
                .ToList();

            return seats ?? [];
        }

        public IEnumerable<SeatPosition> GetSeatPositionsForParent(Guid seatPositionId)
        {
            // returns one level only

            var seatPositions = _context.SeatPositions
                .Include(e => e.SeatPositionType)
                .Where(e => e.ParentId == seatPositionId)
                .OrderBy(e => e.OrdinalValue)
                .ToList();

            return seatPositions ?? [];
        }

        public IEnumerable<SeatPosition> GetSeatPositionsForSeat(Guid parentId)
        {
            // recursively walks back up the chain through the parent id
            // to obtain the full list

            var seatPositionList = new List<SeatPosition>();

            var parent = _context.SeatPositions
                .FirstOrDefault(s => s.Id == parentId);

            if (parent is null)
            {
                return seatPositionList;
            }

            GetSeatPositionNode(parent, ref seatPositionList);

            if (seatPositionList.Count != 0)
            {
                return seatPositionList.OrderBy(s => s.OrdinalValue);
            }

            return seatPositionList;
        }

        private void GetSeatPositionNode(SeatPosition node, ref List<SeatPosition> seatPositionList)
        {
            seatPositionList.Add(node);

            var parent = _context.SeatPositions
                .FirstOrDefault(s => s.Id == node.ParentId);

            if (parent != null)
            {
                GetSeatPositionNode(parent, ref seatPositionList);
            }
        }

        #endregion

        #region Ticket

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            var ticketList = await _context.Tickets
                .Include(e => e.TicketStatus)
                .Include(e => e.Performance)
                .Include(e => e.Performance.Event)
                .Include(e => e.Performance.Venue)
                .Include(e => e.Seat)
                .OrderBy(e => e.Number)
                .ToListAsync();

            return ticketList ?? [];
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync(TicketResourceParameters resourceParms)
        {
            ArgumentNullException.ThrowIfNull(resourceParms);

            var performanceId = resourceParms.PerformanceId;
            var ticketNumber = resourceParms.Number.FormatForSearch();
            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var ticketList = await GetTicketsAsync();

            if (performanceId == null &&
                string.IsNullOrWhiteSpace(ticketNumber) &&
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return ticketList;
            }

            // filter on PerformanceId
            if (performanceId != null)
            {
                ticketList = ticketList.Where(t => t.PerformanceId == performanceId);
            }

            // filter on Ticket Number
            if (!string.IsNullOrWhiteSpace(ticketNumber))
            {
                ticketList = ticketList
                    .Where(t => string.Equals(t.Number, ticketNumber.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // search on Ticket Number
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                ticketList = ticketList.Where(s =>
                    s.Number.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return ticketList;
        }

        public async Task<Ticket?> GetTicketByIdAsync(Guid ticketId)
        {
            return await _context.Tickets
                .Include(e => e.TicketStatus)
                .Include(e => e.Performance)
                .Include(e => e.Performance.Event)
                .Include(e => e.Performance.Venue)
                .Include(e => e.Seat)
                .FirstOrDefaultAsync(e => e.Id == ticketId);
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket);

            await _context.Tickets.AddAsync(ticket);
        }

        public void DeleteTicket(Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket);

            _context.Tickets.Remove(ticket);
        }

        public PerformanceTicketCount GetPerformanceTicketCount(Guid performanceId)
        {
            var performanceTicketCount = new PerformanceTicketCount()
            {
                PerformanceId = performanceId,
                TotalTickets = 0,
                RemainingTickets = 0
            };

            var performanceTickets =
                _context.Tickets.Where(t => t.PerformanceId == performanceId);

            if (performanceTickets.Any())
            {
                performanceTicketCount.TotalTickets = performanceTickets.Count();

                performanceTicketCount.RemainingTickets =
                    performanceTickets.Where(t => t.TicketStatusId == TicketStatusEnum.UnSold).Count();
            }

            return performanceTicketCount;
        }

        #endregion

        #region Seat

        public async Task<IEnumerable<Seat>> GetSeatsAsync()
        {
            return await _context.Seats
                .Include(s => s.SeatType)
                .ToListAsync();
        }

        public async Task<Seat?> GetSeatByIdAsync(Guid seatId)
        {
            return await _context.Seats
                .Include(s => s.SeatType)
                .FirstOrDefaultAsync(s => s.Id == seatId);
        }

        public async Task AddSeatAsync(Seat seat)
        {
            ArgumentNullException.ThrowIfNull(seat);

            await _context.Seats.AddAsync(seat);
        }

        public void DeleteSeat(Seat seat)
        {
            ArgumentNullException.ThrowIfNull(seat);

            _context.Seats.Remove(seat);
        }

        public async Task<bool> SeatExistsAsync(Guid seatId)
        {
            return await _context.Seats
                .AnyAsync(s => s.Id == seatId);
        }

        #endregion

        #region ShoppingCart

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCartsAsync()
        {
            return await _context.ShoppingCarts
                .Include(s => s.ShoppingCartItems)
                .ThenInclude(s => s.Ticket)
                .ToListAsync();
        }

        public async Task<ShoppingCart?> GetShoppingCartByIdAsync(Guid shoppingCartId)
        {
            return await _context.ShoppingCarts
                .Include(s => s.ShoppingCartItems)
                .ThenInclude(s => s.Ticket)
                .FirstOrDefaultAsync(s => s.Id == shoppingCartId);
        }

        public async Task<ShoppingCart?> GetShoppingCartByMemberIdAsync(Guid memberId)
        {
            return await _context.ShoppingCarts
                .Include(s => s.ShoppingCartItems)
                .ThenInclude(s => s.Ticket)
                .FirstOrDefaultAsync(s => s.MemberId == memberId);
        }

        public async Task AddShoppingCartAsync(ShoppingCart shoppingCart)
        {
            ArgumentNullException.ThrowIfNull(shoppingCart);

            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public void DeleteShoppingCart(ShoppingCart shoppingCart)
        {
            ArgumentNullException.ThrowIfNull(shoppingCart);

            _context.ShoppingCarts.Remove(shoppingCart);
        }

        public async Task<bool> ShoppingCartExistsAsync(Guid shoppingCartId)
        {
            return await _context.ShoppingCarts
                .AnyAsync(s => s.Id == shoppingCartId);
        }

        public async Task<ShoppingCartItem?> GetShoppingCartItemByIdAsync(Guid shoppingCartItemId)
        {
            return await _context.ShoppingCartItems
                .Include(s => s.Ticket)
                .FirstOrDefaultAsync(s => s.Id == shoppingCartItemId);
        }

        public async Task AddShoppingCartItemAsync(ShoppingCartItem shoppingCartItem)
        {
            ArgumentNullException.ThrowIfNull(shoppingCartItem);

            await _context.ShoppingCartItems.AddAsync(shoppingCartItem);
        }

        public void DeleteShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            ArgumentNullException.ThrowIfNull(shoppingCartItem);

            _context.ShoppingCartItems.Remove(shoppingCartItem);
        }

        #endregion

        #region Member

        public async Task<IEnumerable<Member>> GetMembersAsync()
        {
            var memberList = await _context.Members
                .Include(m => m.ShoppingCart)
                .Include(m => m.Orders)
                .Where(m => m.Username != null)
                .ToListAsync();

            foreach (var member in memberList)
            {
                member.MemberAssets = await GetMemberAssetsAsync(member.Id);
            }

            return memberList;
        }

        public async Task<IEnumerable<Member>> GetMembersAsync(MemberResourceParameters resourceParms)
        {
            ArgumentNullException.ThrowIfNull(resourceParms);

            var username = resourceParms.Username.FormatForSearch();
            var fullName = resourceParms.FullName.FormatForSearch();
            var emailAddress = resourceParms.EmailAddress.FormatForSearch();
            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var memberList = await GetMembersAsync();

            if (string.IsNullOrWhiteSpace(username) &&
                string.IsNullOrWhiteSpace(fullName) &&
                string.IsNullOrWhiteSpace(emailAddress) &&
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return memberList;
            }

            // filters

            if (!string.IsNullOrWhiteSpace(username))
            {
                memberList = memberList
                    .Where(m => string.Equals(m.Username, username.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                memberList = memberList
                    .Where(m => string.Equals(m.FullName, fullName.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                memberList = memberList
                    .Where(m => string.Equals(m.EmailAddress, emailAddress.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            // search

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();

                memberList = memberList.Where(s =>
                        s.Username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                        s.FullName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                        s.EmailAddress.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return memberList;
        }

        public async Task<Member?> GetMemberByIdAsync(Guid memberId)
        {
            var member = await _context.Members
                .Include(m => m.ShoppingCart)
                .FirstOrDefaultAsync(m => m.Id == memberId);

            if (member != null)
            {
                member.MemberAssets = await GetMemberAssetsAsync(member.Id);
            }

            return member;
        }

        public async Task<Member?> GetMemberByMembershipNumberAsync(string membershipNumber)
        {
            var member = await _context.Members
                .Include(m => m.ShoppingCart)
                .FirstOrDefaultAsync(m => m.MembershipNumber == membershipNumber);

            if (member is not null)
            {
                member.MemberAssets = await GetMemberAssetsAsync(member.Id);
            }

            return member;
        }

        public async Task AddMemberAsync(Member member)
        {
            ArgumentNullException.ThrowIfNull(member);

            await _context.Members.AddAsync(member);
        }

        public void DeleteMember(Member member)
        {
            ArgumentNullException.ThrowIfNull(member);

            _context.Members.Remove(member);
        }

        public async Task<bool> MemberExistsAsync(Guid memberId)
        {
            return await _context.Members
                .AnyAsync(m => m.Id == memberId);
        }

        public void SoftDeleteMember(Member member)
        {
            if (member is not null)
            {
                member.IsActive = false;
            }
        }

        public async Task<List<MemberAsset>> GetMemberAssetsAsync(Guid memberId)
        {
            return await _context.MemberAssets
                .Include(m => m.AssetType)
                .Include(m => m.Asset)
                .Include(m => m.Member)
                .Where(m => m.MemberId == memberId)
                .OrderBy(m => m.AssetType.Name)
                .ThenBy(m => m.OrdinalValue)
                .ToListAsync();
        }

        public async Task<IEnumerable<MemberAsset>> GetMemberAssetsAsync(Guid memberId,
            AssetResourceParameters resourceParms)
        {
            if (resourceParms.AssetTypeName is null)
            {
                return await GetMemberAssetsAsync(memberId);
            }

            if (Enum.TryParse<AssetTypeEnum>(resourceParms.AssetTypeName, true, out AssetTypeEnum parsedValue))
            {
                return await _context.MemberAssets
                   .Include(m => m.AssetType)
                   .Include(m => m.Asset)
                   .Include(m => m.Member)
                   .Where(m => m.MemberId == memberId && m.AssetTypeId == parsedValue)
                   .OrderBy(m => m.OrdinalValue)
                   .ToListAsync();
            }

            return [];
        }

        public async Task<MemberAsset?> GetMemberAssetByIdAsync(Guid memberAssetId)
        {
            return await _context.MemberAssets
                .Include(m => m.AssetType)
                .Include(m => m.Asset)
                .Include(m => m.Member)
                .FirstOrDefaultAsync(m => m.Id == memberAssetId);
        }

        public async Task AddMemberAssetAsync(MemberAsset memberAsset)
        {
            ArgumentNullException.ThrowIfNull(memberAsset);

            if (await MemberExistsAsync(memberAsset.MemberId) &&
                await AssetExistsAsync(memberAsset.AssetId))
            {
                await _context.MemberAssets.AddAsync(memberAsset);
            }
        }

        public void DeleteMemberAsset(MemberAsset memberAsset)
        {
            _context.MemberAssets.Remove(memberAsset);
        }

        public async Task<bool> MemberAssetExistsAsync(Guid memberId, AssetTypeEnum assetType)
        {
            return await _context.MemberAssets.AnyAsync(m =>
                m.MemberId == memberId &&
                m.AssetTypeId == assetType);
        }

        #endregion

        #region Order

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Ticket)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(OrderResourceParameters resourceParms)
        {
            ArgumentNullException.ThrowIfNull(resourceParms);

            var orderStatus = resourceParms.OrderStatus.FormatForSearch();
            var searchQuery = resourceParms.SearchQuery.FormatForSearch();

            var orderList = await GetOrdersAsync();

            if (string.IsNullOrWhiteSpace(orderStatus) &&
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return orderList;
            }

            // filter

            if (!string.IsNullOrWhiteSpace(orderStatus))
            {
                orderList = orderList
                    .Where(o => string.Equals(o.OrderStatusId.ToString(), orderStatus.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            // search

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();

                var matchingMembers = await GetMembersAsync(new MemberResourceParameters
                {
                    SearchQuery = searchQuery
                });

                if (matchingMembers != null)
                {
                    var memberIds = matchingMembers.Select(m => m.Id).Distinct();

                    orderList = orderList.Where(s =>
                        memberIds.Contains(s.MemberId) ||
                        s.Date.ToShortDateString().Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                        s.OrderStatusId.ToString().Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    orderList = orderList.Where(s =>
                        s.Date.ToShortDateString().Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                        s.OrderStatusId.ToString().Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                }
            }

            return orderList;
        }

        public async Task<IEnumerable<Order>> GetOrdersForMemberAsync(Guid memberId)
        {
            return await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Ticket)
                .Where(o => o.MemberId == memberId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Ticket)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task AddOrderAsync(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            await _context.Orders.AddAsync(order);
        }

        public void DeleteOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            _context.Orders.Remove(order);
        }

        public async Task<bool> OrderExistsAsync(Guid orderId)
        {
            return await _context.Orders
                .AnyAsync(o => o.Id == orderId);
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId)
        {
            return await _context.OrderItems
                .Include(o => o.Ticket)
                .FirstOrDefaultAsync(o => o.Id == orderItemId);
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            ArgumentNullException.ThrowIfNull(orderItem);

            await _context.OrderItems.AddAsync(orderItem);
        }

        public void DeleteOrderItem(OrderItem orderItem)
        {
            ArgumentNullException.ThrowIfNull(orderItem);

            _context.OrderItems.Remove(orderItem);
        }

        #endregion

        public async Task<bool> SaveChangesAsync()
        {
            // for debugging, watch:
            // _context.ChangeTracker.Entries(), results

            _context.ChangeTracker.DetectChanges();

            return await _context.SaveChangesAsync() >= 0;
        }
    }
}

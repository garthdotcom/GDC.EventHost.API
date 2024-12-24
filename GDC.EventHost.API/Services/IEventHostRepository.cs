using GDC.EventHost.API.Entities;
using GDC.EventHost.API.ResourceParameters;

namespace GDC.EventHost.API.Services
{
    public interface IEventHostRepository
    {
        // Get All

        Task<IEnumerable<Series>> GetSeriesAsync();

        Task<IEnumerable<Event>> GetEventsAsync();

        Task<IEnumerable<Venue>> GetVenuesAsync();

        Task<(IEnumerable<Series>, PaginationMetadata)> GetSeriesAsync(
            SeriesResourceParameters seriesResourceParameters);

        Task<(IEnumerable<Event>, PaginationMetadata)> GetEventsAsync(
            EventResourceParameters eventResourceParameters);

        Task<IEnumerable<Event>> GetEventsForSeriesAsync(Guid seriesId, bool includeDetail);

        Task<IEnumerable<Performance>> GetPerformancesForEventAsync(Guid eventId, bool includeDetail);

        Task<(IEnumerable<Venue>, PaginationMetadata)> GetVenuesAsync(
            VenueResourceParameters venueResourceParameters);


        // Get Single

        Task<Series?> GetSeriesAsync(Guid seriesId, bool includeDetail);

        Task<Event?> GetEventAsync(Guid eventId, bool includeDetail = false);

        Task<Event?> GetEventForSeriesAsync(Guid seriesId, Guid eventId, bool includeDetail);

        Task<Performance?> GetPerformanceForEventAsync(Guid eventId, Guid performanceId, bool includeDetail);

        Task<Venue?> GetVenueAsync(Guid venueId, bool includeDetail);


        // Add

        void AddSeries(Series series);

        Task AddEventToSeriesAsync(Guid seriesId, Event theEvent);

        Task AddPerformanceToEventAsync(Guid eventId, Performance performance);

        void AddVenue(Venue venue);


        // Delete

        void DeleteSeries(Series series);

        void DeleteEvent(Event theEvent);

        void DeletePerformance(Performance performance);

        void DeleteVenue(Venue venue);


        // Exists

        Task<bool> SeriesExistsAsync(Guid seriesId);

        Task<bool> EventExistsAsync(Guid eventId);

        Task<bool> VenueExistsAsync(Guid venueId);


        // Persist

        Task<bool> SaveChangesAsync();
    }
}

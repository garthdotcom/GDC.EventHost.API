using GDC.EventHost.API.Entities;
using GDC.EventHost.API.ResourceParameters;

namespace GDC.EventHost.API.Services
{
    public interface IEventHostRepository
    {
        // Get All

        Task<IEnumerable<Series>> GetSeriesAsync();

        Task<IEnumerable<Event>> GetEventsAsync();

        Task<(IEnumerable<Series>, PaginationMetadata)> GetSeriesAsync(
            SeriesResourceParameters seriesResourceParameters);

        Task<(IEnumerable<Event>, PaginationMetadata)> GetEventsAsync(
            EventResourceParameters eventResourceParameters);

        Task<IEnumerable<Event>> GetEventsForSeriesAsync(Guid seriesId, bool includeDetail);

        Task<IEnumerable<Performance>> GetPerformancesForEventAsync(Guid eventId, bool includeDetail);


        // Get Single

        Task<Series?> GetSeriesAsync(Guid seriesId, bool includeDetail);

        Task<Event?> GetEventAsync(Guid eventId, bool includeDetail = false);

        Task<Event?> GetEventForSeriesAsync(Guid seriesId, Guid eventId, bool includeDetail);

        Task<Performance?> GetPerformanceForEventAsync(Guid eventId, Guid performanceId, bool includeDetail);


        // Add

        void AddSeries(Series series);

        Task AddEventToSeriesAsync(Guid seriesId, Event theEvent);

        Task AddPerformanceToEventAsync(Guid eventId, Performance performance);


        // Delete

        void DeleteSeries(Series series);

        void DeleteEvent(Event theEvent);

        void DeletePerformance(Performance performance);

        
        // Exists

        Task<bool> SeriesExistsAsync(Guid seriesId);

        Task<bool> EventExistsAsync(Guid eventId);

        
        // Persist

        Task<bool> SaveChangesAsync();
    }
}

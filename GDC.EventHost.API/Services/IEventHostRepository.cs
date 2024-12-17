using GDC.EventHost.API.Entities;

namespace GDC.EventHost.API.Services
{
    public interface IEventHostRepository
    {
        // Get All

        Task<IEnumerable<Series>> GetSeriesAsync();

        Task<IEnumerable<Event>> GetEventsAsync();

        Task<(IEnumerable<Series>, PaginationMetadata)> GetSeriesAsync(
            string? title, string? searchQuery, int pageNumber, int pageSize);

        Task<(IEnumerable<Event>, PaginationMetadata)> GetEventsAsync(
            string? title, string? searchQuery, int pageNumber, int pageSize);

        Task<IEnumerable<Event>> GetEventsForSeriesAsync(Guid seriesId);

        Task<IEnumerable<Performance>> GetPerformancesForEventAsync(Guid eventId);


        // Get Single

        Task<Series?> GetSeriesAsync(Guid seriesId, bool includeEvents);

        Task<Event?> GetEventAsync(Guid eventId, bool includePerformances);

        Task<Event?> GetEventForSeriesAsync(Guid seriesId, Guid eventId);

        Task<Performance?> GetPerformanceForEventAsync(Guid eventId, Guid performanceId);


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

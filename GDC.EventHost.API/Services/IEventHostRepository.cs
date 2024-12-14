using GDC.EventHost.API.Entities;

namespace GDC.EventHost.API.Services
{
    public interface IEventHostRepository
    {
        Task<IEnumerable<Event>> GetEventsAsync();

        Task<Event?> GetEventAsync(Guid eventId, bool includePerformances);

        Task<IEnumerable<Performance>> GetPerformancesForEventAsync(Guid eventId);

        Task<Performance?> GetPerformanceForEventAsync(Guid eventId, Guid performanceId);

        Task<bool> EventExistsAsync(Guid eventId);

        Task AddPerformanceToEventAsync(Guid eventId, Performance performance);

        void DeletePerformance(Performance performance);

        Task<bool> SaveChangesAsync();
    }
}

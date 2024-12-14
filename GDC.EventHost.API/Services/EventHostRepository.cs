using GDC.EventHost.API.DbContexts;
using GDC.EventHost.API.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddPerformanceToEventAsync(Guid eventId, Performance performance)
        {
            var existingEvent = await GetEventAsync(eventId, false);

            if (existingEvent != null)
            {
                existingEvent.Performances.Add(performance);
            }
        }

        public void DeletePerformance(Performance performance)
        {
            _context.Performances.Remove(performance);
        }

        public async Task<bool> EventExistsAsync(Guid eventId)
        {
            return await _context.Events.AnyAsync(e => e.Id == eventId);
        }

        public async Task<Event?> GetEventAsync(Guid eventId, bool includePerformances)
        {
            if (includePerformances)
            {
                return await _context.Events
                    .Include(e => e.Performances)
                    .Where(e => e.Id == eventId)
                    .FirstOrDefaultAsync();
            }

            return await _context.Events
                .Where(e => e.Id == eventId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            return await _context.Events
                .OrderBy(e => e.Title)
                .ToListAsync();
        }

        public async Task<Performance?> GetPerformanceForEventAsync(Guid eventId, Guid performanceId)
        {
            return await _context.Performances
                .Where(p => p.Id == performanceId && p.EventId == eventId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Performance>> GetPerformancesForEventAsync(Guid eventId)
        {
            return await _context.Performances
                .Where(p => p.EventId == eventId)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}

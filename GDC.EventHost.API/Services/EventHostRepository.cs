using AutoMapper;
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

        public async Task<(IEnumerable<Event>, PaginationMetadata)> GetEventsAsync(
            string? title, string? searchQuery, int pageNumber, int pageSize)
        {
            // collection to start from; build the expression tree
            var collection = _context.Events as IQueryable<Event>;

            if (!string.IsNullOrWhiteSpace(title))
            {
                title = title.Trim();
                collection = collection.Where(e => e.Title == title);
            }
            
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {  
                searchQuery = searchQuery.Trim();
                collection = collection.Where(e => e.Title.Contains(searchQuery, 
                    StringComparison.InvariantCultureIgnoreCase));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            // execute the query
            var collectionToReturn = await collection.OrderBy(e => e.Title)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
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

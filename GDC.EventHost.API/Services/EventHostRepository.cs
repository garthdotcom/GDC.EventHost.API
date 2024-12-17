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

        // Get All

        public async Task<IEnumerable<Series>> GetSeriesAsync()
        {
            return await _context.Series
                .OrderBy(e => e.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            return await _context.Events
                .OrderBy(e => e.Title)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Series>, PaginationMetadata)> GetSeriesAsync(
            string? title, string? searchQuery, int pageNumber, int pageSize)
        {
            // collection to start from; build the expression tree
            var collection = _context.Series as IQueryable<Series>;

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

        public async Task<IEnumerable<Event>> GetEventsForSeriesAsync(Guid seriesId)
        {
            return await _context.Events
                .Where(e => e.SeriesId == seriesId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Performance>> GetPerformancesForEventAsync(Guid eventId)
        {
            return await _context.Performances
                .Where(p => p.EventId == eventId)
                .ToListAsync();
        }


        // Get Single

        public async Task<Series?> GetSeriesAsync(Guid seriesId, bool includeEvents)
        {
            if (includeEvents)
            {
                return await _context.Series
                .Include(e => e.Events)
                    .Where(e => e.Id == seriesId)
                    .FirstOrDefaultAsync();
            }
            return await _context.Series
                .Where(e => e.Id == seriesId)
                .FirstOrDefaultAsync();
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

        public async Task<Event?> GetEventForSeriesAsync(Guid seriesId, Guid eventId)
        {
            return await _context.Events
                .Where(e => e.Id == eventId && e.SeriesId == seriesId)
                .FirstOrDefaultAsync();
        }

        public async Task<Performance?> GetPerformanceForEventAsync(Guid eventId, Guid performanceId)
        {
            return await _context.Performances
                .Where(p => p.Id == performanceId && p.EventId == eventId)
                .FirstOrDefaultAsync();
        }


        // Add

        public void AddSeries(Series series)
        {
            _context.Series.Add(series);
        }

        public async Task AddEventToSeriesAsync(Guid seriesId, Event theEvent)
        {
            var existingSeries = await GetSeriesAsync(seriesId, false);

            if (existingSeries != null)
            {
                existingSeries.Events.Add(theEvent);
            }
        }

        public async Task AddPerformanceToEventAsync(Guid eventId, Performance performance)
        {
            var existingEvent = await GetEventAsync(eventId, false);

            if (existingEvent != null)
            {
                existingEvent.Performances.Add(performance);
            }
        }


        // Delete

        public void DeleteSeries(Series series)
        {
            _context.Series.Remove(series);
        }

        public void DeleteEvent(Event theEvent)
        {
            _context.Events.Remove(theEvent);
        }

        public void DeletePerformance(Performance performance)
        {
            _context.Performances.Remove(performance);
        }


        // Exists

        public async Task<bool> SeriesExistsAsync(Guid seriesId)
        {
            return await _context.Series.AnyAsync(s => s.Id == seriesId);
        }

        public async Task<bool> EventExistsAsync(Guid eventId)
        {
            return await _context.Events.AnyAsync(e => e.Id == eventId);
        }


        // Persist

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
   
    }
}

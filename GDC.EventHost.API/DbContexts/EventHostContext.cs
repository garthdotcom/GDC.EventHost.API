using GDC.EventHost.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GDC.EventHost.API.DbContexts
{
    public class EventHostContext : DbContext
    {
        public DbSet<Series> Series { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<PerformanceType> PerformanceTypes { get; set; }


        public EventHostContext(DbContextOptions<EventHostContext> options)
            : base(options)
        {
            
        }
    }
}

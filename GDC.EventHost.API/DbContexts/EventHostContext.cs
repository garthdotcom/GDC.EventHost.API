using GDC.EventHost.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GDC.EventHost.API.DbContexts
{
    public class EventHostContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<Performance> Performances { get; set; }

        public EventHostContext(DbContextOptions<EventHostContext> options)
            : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionstring");

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}

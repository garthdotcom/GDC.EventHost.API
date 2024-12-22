using GDC.EventHost.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.API.DbContexts
{
    public class EventHostContext : DbContext
    {
        public DbSet<Series> Series { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<PerformanceType> PerformanceTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }


        public EventHostContext(DbContextOptions<EventHostContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // disable identity for lookup table Ids
            modelBuilder.Entity<Status>().Property(s => s.Id).ValueGeneratedNever();

            // prevent cascade to lookup table if parent table row is deleted
            modelBuilder.Entity<Series>().HasOne(e => e.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Event>().HasOne(e => e.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Performance>().HasOne(e => e.Status).WithMany().OnDelete(DeleteBehavior.Restrict);

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                // add auditing properties

                if (!entityType.IsOwned())
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedDate").IsRequired(true);
                    modelBuilder.Entity(entityType.Name).Property<string>("CreatedBy").IsRequired(true);
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("LastUpdatedDate").IsRequired(false);
                    modelBuilder.Entity(entityType.Name).Property<string>("LastUpdatedBy").IsRequired(false);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            // TODO: Get username from session or other authentication
            var userName = "session-user";

            foreach (var entity in ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
            {
                entity.Property("CreatedDate").CurrentValue = DateTime.Now;
                entity.Property("CreatedBy").CurrentValue = userName;
                entity.Property("LastUpdatedDate").CurrentValue = null;
                entity.Property("LastUpdatedBy").CurrentValue = null;
            }

            foreach (var entity in ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                entity.Property("LastUpdatedDate").CurrentValue = DateTime.Now;
                entity.Property("LastUpdatedBy").CurrentValue = userName;
            }

            return base.SaveChanges();
        }
    }
}

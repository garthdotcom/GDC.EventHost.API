using GDC.EventHost.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GDC.EventHost.DAL.DbContexts
{
    public class EventHostContext : DbContext
    {
        public DbSet<Series> Series { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<PerformanceType> PerformanceTypes { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<SeatingPlan> SeatingPlans { get; set; }
        public DbSet<SeatPosition> SeatPositions { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<ContactInfoType> ContactInfoTypes { get; set; }
        public DbSet<PerformanceContactInfo> PerformanceContactInfos { get; set; }
        public DbSet<VenueContactInfo> VenueContactInfos { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<SeriesAsset> SeriesAssets { get; set; }
        public DbSet<EventAsset> EventAssets { get; set; }
        public DbSet<PerformanceAsset> PerformanceAssets { get; set; }
        public DbSet<VenueAsset> VenueAssets { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberAsset> MemberAssets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<SeatPositionType> SeatPositionTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }


        public EventHostContext(DbContextOptions<EventHostContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // disable identity for lookup table Ids
            modelBuilder.Entity<Status>().Property(s => s.Id).ValueGeneratedNever();
            modelBuilder.Entity<SeatType>().Property(s => s.Id).ValueGeneratedNever();
            modelBuilder.Entity<SeatPositionType>().Property(s => s.Id).ValueGeneratedNever();
            modelBuilder.Entity<TicketStatus>().Property(t => t.Id).ValueGeneratedNever();
            modelBuilder.Entity<AssetType>().Property(a => a.Id).ValueGeneratedNever();
            modelBuilder.Entity<OrderStatus>().Property(o => o.Id).ValueGeneratedNever();

            // define composite keys for join tables
            modelBuilder.Entity<PerformanceContactInfo>().HasKey(p => new { p.PerformanceId, p.ContactInfoId });
            modelBuilder.Entity<VenueContactInfo>().HasKey(v => new { v.VenueId, v.ContactInfoId });
            modelBuilder.Entity<Ticket>().Property(t => t.Price).HasColumnType("decimal(9,2)");

            // prevent cascade to venue if a seating plan is deleted
            modelBuilder.Entity<SeatingPlan>().HasOne(s => s.Venue).WithMany(v => v.SeatingPlans).OnDelete(DeleteBehavior.Restrict);

            // prevent cascade to seat if a ticket is deleted
            modelBuilder.Entity<Ticket>().HasOne(t => t.Seat).WithMany().OnDelete(DeleteBehavior.Restrict);

            // prevent cascade to lookup table if parent table row is deleted
            modelBuilder.Entity<Event>().HasOne(e => e.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Performance>().HasOne(p => p.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Venue>().HasOne(v => v.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeatingPlan>().HasOne(s => s.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ticket>().HasOne(t => t.TicketStatus).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Seat>().HasOne(s => s.SeatType).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeatPosition>().HasOne(s => s.SeatPositionType).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Member>().HasOne(m => m.ShoppingCart).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ticket>().HasOne(t => t.Seat).WithMany(s => s.Tickets).HasForeignKey(t => t.SeatId);

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                // adding shadow properties for auditing

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Get username from session or other authentication
            var userName = "session-user";

            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                entity.Property("CreatedDate").CurrentValue = DateTime.Now;
                entity.Property("CreatedBy").CurrentValue = userName;
                entity.Property("LastUpdatedDate").CurrentValue = null;
                entity.Property("LastUpdatedBy").CurrentValue = null;
            }

            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                entity.Property("LastUpdatedDate").CurrentValue = DateTime.Now;
                entity.Property("LastUpdatedBy").CurrentValue = userName;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

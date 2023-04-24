using Microsoft.EntityFrameworkCore;

namespace CentreBookingDatabase.Models
{
    public class CentreBookingContext : DbContext
    {
        public CentreBookingContext(DbContextOptions<CentreBookingContext> options)
            : base(options)
        {
        }

        public DbSet<Centre> Centres { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /**
             * defining Centre and Booking relationship in the Model
             */
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Centre) // Booking is linked to Centre
                .WithMany(c => c.Bookings) // A centre can have more than one booking(s)
                .HasForeignKey(b => b.CentreName); // Booking has a foreign key link to Centre 

            modelBuilder.Entity<Booking>()
                .Property(b => b.StartDate)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToString("yyyy-MM-dd") : null,
                    v => v != null ? DateOnly.Parse(v) : null);

            modelBuilder.Entity<Booking>()
                .Property(b => b.EndDate)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToString("yyyy-MM-dd") : null,
                    v => v != null ? DateOnly.Parse(v) : null);
        }
    }
}

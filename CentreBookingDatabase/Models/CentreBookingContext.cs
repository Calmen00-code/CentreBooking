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
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Centre)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CentreName);
        }
    }
}

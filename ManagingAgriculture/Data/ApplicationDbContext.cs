using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ManagingAgriculture.Models;

namespace ManagingAgriculture.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyInvitation> CompanyInvitations { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Machinery> Machinery { get; set; }
        public DbSet<MarketplaceListing> MarketplaceListings { get; set; }
        public DbSet<ResourceUsage> ResourceUsages { get; set; }
        public DbSet<MaintenanceHistory> MaintenanceHistory { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Machinery <-> MarketplaceListing relationship
            builder.Entity<MarketplaceListing>()
                .HasOne(m => m.Machinery)
                .WithMany(m => m.MarketplaceListings)
                .HasForeignKey(m => m.MachineryId)
                .OnDelete(DeleteBehavior.SetNull); // Or Restrict, depending on requirements. SetNull is safer if machinery is deleted but listing remains (though unlikely).

            // Configure decimal precision for currency and other decimal fields to avoid warnings
            builder.Entity<Plant>()
                .Property(p => p.AvgTemperatureCelsius)
                .HasColumnType("decimal(5,2)");

            builder.Entity<Resource>()
                .Property(r => r.Quantity)
                .HasColumnType("decimal(10,2)");

            builder.Entity<Resource>()
                .Property(r => r.LowStockThreshold)
                .HasColumnType("decimal(10,2)");

            builder.Entity<Machinery>()
                .Property(m => m.PurchasePrice)
                .HasColumnType("decimal(10,2)");

            builder.Entity<Machinery>()
                .Property(m => m.EngineHours)
                .HasColumnType("decimal(10,1)");

            builder.Entity<MarketplaceListing>()
                .Property(m => m.SalePrice)
                .HasColumnType("decimal(10,2)");

            builder.Entity<MarketplaceListing>()
                .Property(m => m.RentalPricePerDay)
                .HasColumnType("decimal(10,2)");

            builder.Entity<MarketplaceListing>()
                .Property(m => m.EngineHours)
                .HasColumnType("decimal(10,1)");

            builder.Entity<ResourceUsage>()
                .Property(r => r.QuantityUsed)
                .HasColumnType("decimal(10,2)");

            builder.Entity<MaintenanceHistory>()
                .Property(m => m.Cost)
                .HasColumnType("decimal(10,2)");
        }
    }
}

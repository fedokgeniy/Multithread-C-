using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Data;

/// <summary>
/// Database context using Table-Per-Concrete-Type (TPC) inheritance strategy.
/// Each concrete type is mapped to its own table containing all properties.
/// </summary>
public class TpcPhoneContext : BasePhoneContext
{
    /// <summary>
    /// Initializes a new instance of the TpcPhoneContext class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public TpcPhoneContext(DbContextOptions<TpcPhoneContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configures the model using TPC inheritance strategy.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCommonEntities(modelBuilder);

        // Configure TPC inheritance strategy
        modelBuilder.Entity<Phone>().UseTpcMappingStrategy();

        // Configure tables for concrete types only
        modelBuilder.Entity<FeaturePhone>().ToTable("FeaturePhones");
        modelBuilder.Entity<Smartphone>().ToTable("Smartphones");
        modelBuilder.Entity<GamingPhone>().ToTable("GamingPhones");

        // Configure FeaturePhone specific properties
        modelBuilder.Entity<FeaturePhone>(entity =>
        {
            entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ScreenSize).HasColumnType("decimal(3,1)");
            entity.Property(e => e.HasPhysicalKeypad);
            entity.Property(e => e.SmsStorageCapacity);
            entity.Property(e => e.SupportsBasicGames);

            // Configure relationship with Manufacturer for FeaturePhone
            entity.HasOne(e => e.Manufacturer)
                  .WithMany()
                  .HasForeignKey(e => e.ManufacturerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Smartphone specific properties
        modelBuilder.Entity<Smartphone>(entity =>
        {
            entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ScreenSize).HasColumnType("decimal(3,1)");
            entity.Property(e => e.OperatingSystem).IsRequired().HasMaxLength(50);
            entity.Property(e => e.RamCapacity);
            entity.Property(e => e.StorageCapacity);
            entity.Property(e => e.CameraResolution).HasColumnType("decimal(4,1)");
            entity.Property(e => e.Supports5G);

            // Configure relationship with Manufacturer for Smartphone
            entity.HasOne(e => e.Manufacturer)
                  .WithMany()
                  .HasForeignKey(e => e.ManufacturerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure GamingPhone specific properties
        modelBuilder.Entity<GamingPhone>(entity =>
        {
            entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ScreenSize).HasColumnType("decimal(3,1)");
            entity.Property(e => e.OperatingSystem).IsRequired().HasMaxLength(50);
            entity.Property(e => e.RamCapacity);
            entity.Property(e => e.StorageCapacity);
            entity.Property(e => e.GpuModel).IsRequired().HasMaxLength(100);
            entity.Property(e => e.RefreshRate);
            entity.Property(e => e.HasGamingTriggers);
            entity.Property(e => e.CoolingSystem).HasMaxLength(100);

            // Configure relationship with Manufacturer for GamingPhone
            entity.HasOne(e => e.Manufacturer)
                  .WithMany()
                  .HasForeignKey(e => e.ManufacturerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
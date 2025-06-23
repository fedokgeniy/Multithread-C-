using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Data;

/// <summary>
/// Database context using Table-Per-Hierarchy (TPH) inheritance strategy.
/// All phone types are stored in a single table with a discriminator column.
/// </summary>
public class TphPhoneContext : BasePhoneContext
{
    /// <summary>
    /// Initializes a new instance of the TphPhoneContext class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public TphPhoneContext(DbContextOptions<TphPhoneContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configures the model using TPH inheritance strategy.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCommonEntities(modelBuilder);

        // Configure TPH inheritance strategy
        modelBuilder.Entity<Phone>()
            .HasDiscriminator<string>("PhoneType")
            .HasValue<FeaturePhone>("FeaturePhone")
            .HasValue<Smartphone>("Smartphone")
            .HasValue<GamingPhone>("GamingPhone");

        // Configure FeaturePhone specific properties
        modelBuilder.Entity<FeaturePhone>(entity =>
        {
            entity.Property(e => e.HasPhysicalKeypad);
            entity.Property(e => e.SmsStorageCapacity);
            entity.Property(e => e.SupportsBasicGames);
        });

        // Configure Smartphone specific properties
        modelBuilder.Entity<Smartphone>(entity =>
        {
            entity.Property(e => e.OperatingSystem).HasMaxLength(50);
            entity.Property(e => e.RamCapacity);
            entity.Property(e => e.StorageCapacity);
            entity.Property(e => e.CameraResolution).HasColumnType("decimal(4,1)");
            entity.Property(e => e.Supports5G);
        });

        // Configure GamingPhone specific properties
        modelBuilder.Entity<GamingPhone>(entity =>
        {
            entity.Property(e => e.OperatingSystem).HasMaxLength(50);
            entity.Property(e => e.RamCapacity);
            entity.Property(e => e.StorageCapacity);
            entity.Property(e => e.GpuModel).HasMaxLength(100);
            entity.Property(e => e.RefreshRate);
            entity.Property(e => e.HasGamingTriggers);
            entity.Property(e => e.CoolingSystem).HasMaxLength(100);
        });
    }
}
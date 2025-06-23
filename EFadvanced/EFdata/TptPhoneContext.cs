using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Data;

/// <summary>
/// Database context using Table-Per-Type (TPT) inheritance strategy.
/// Each type in the inheritance hierarchy is mapped to a separate table.
/// </summary>
public class TptPhoneContext : BasePhoneContext
{
    /// <summary>
    /// Initializes a new instance of the TptPhoneContext class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public TptPhoneContext(DbContextOptions<TptPhoneContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configures the model using TPT inheritance strategy.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCommonEntities(modelBuilder);

        // Configure TPT inheritance strategy
        modelBuilder.Entity<Phone>().ToTable("Phones");
        modelBuilder.Entity<FeaturePhone>().ToTable("FeaturePhones");
        modelBuilder.Entity<Smartphone>().ToTable("Smartphones");
        modelBuilder.Entity<GamingPhone>().ToTable("GamingPhones");

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
            entity.Property(e => e.OperatingSystem).IsRequired().HasMaxLength(50);
            entity.Property(e => e.RamCapacity);
            entity.Property(e => e.StorageCapacity);
            entity.Property(e => e.CameraResolution).HasColumnType("decimal(4,1)");
            entity.Property(e => e.Supports5G);
        });

        // Configure GamingPhone specific properties
        modelBuilder.Entity<GamingPhone>(entity =>
        {
            entity.Property(e => e.OperatingSystem).IsRequired().HasMaxLength(50);
            entity.Property(e => e.RamCapacity);
            entity.Property(e => e.StorageCapacity);
            entity.Property(e => e.GpuModel).IsRequired().HasMaxLength(100);
            entity.Property(e => e.RefreshRate);
            entity.Property(e => e.HasGamingTriggers);
            entity.Property(e => e.CoolingSystem).HasMaxLength(100);
        });
    }
}
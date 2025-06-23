using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Data;

/// <summary>
/// Base database context class containing common configurations.
/// </summary>
public abstract class BasePhoneContext : DbContext
{
    /// <summary>
    /// Gets or sets the DbSet for manufacturers.
    /// </summary>
    public DbSet<Manufacturer> Manufacturers { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for phones.
    /// </summary>
    public DbSet<Phone> Phones { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for feature phones.
    /// </summary>
    public DbSet<FeaturePhone> FeaturePhones { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for smartphones.
    /// </summary>
    public DbSet<Smartphone> Smartphones { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for gaming phones.
    /// </summary>
    public DbSet<GamingPhone> GamingPhones { get; set; }

    /// <summary>
    /// Initializes a new instance of the BasePhoneContext class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    protected BasePhoneContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// Configures common entity properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model.</param>
    protected void ConfigureCommonEntities(ModelBuilder modelBuilder)
    {
        // Configure Manufacturer entity
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
        });

        // Configure base Phone entity
        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ScreenSize).HasColumnType("decimal(3,1)");

            // Configure relationship with Manufacturer
            entity.HasOne(e => e.Manufacturer)
                  .WithMany(m => m.Phones)
                  .HasForeignKey(e => e.ManufacturerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
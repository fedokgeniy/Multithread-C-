using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Data
{
    /// <summary>
    /// Base database context for phone entities providing common configuration.
    /// Serves as the foundation for different inheritance strategy implementations.
    /// </summary>
    public abstract class BasePhoneContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the BasePhoneContext class.
        /// </summary>
        protected BasePhoneContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the BasePhoneContext class with options.
        /// </summary>
        /// <param name="options">The options for this context</param>
        protected BasePhoneContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for manufacturers.
        /// </summary>
        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DbSet for phones.
        /// </summary>
        public DbSet<Phone> Phones { get; set; } = null!;

        /// <summary>
        /// Configures the model and relationships using the Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureManufacturer(modelBuilder);
            ConfigurePhoneBase(modelBuilder);
            ConfigureInheritanceStrategy(modelBuilder);
        }

        /// <summary>
        /// Configures the Manufacturer entity.
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected virtual void ConfigureManufacturer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(m => m.ManufacturerId);
                entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Country).HasMaxLength(100);
                entity.HasIndex(m => m.Name).IsUnique();
            });
        }

        /// <summary>
        /// Configures the base Phone entity properties.
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected virtual void ConfigurePhoneBase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phone>(entity =>
            {
                entity.HasKey(p => p.PhoneId);
                entity.Property(p => p.Model).IsRequired().HasMaxLength(100);
                entity.Property(p => p.SerialNumber).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Price).HasPrecision(18, 2);
                entity.HasIndex(p => p.SerialNumber).IsUnique();

                entity.HasOne(p => p.Manufacturer)
                      .WithMany(m => m.Phones)
                      .HasForeignKey(p => p.ManufacturerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        /// <summary>
        /// Configures the inheritance strategy. Must be implemented by derived classes.
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected abstract void ConfigureInheritanceStrategy(ModelBuilder modelBuilder);
    }
}
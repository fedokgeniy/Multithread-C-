using Microsoft.EntityFrameworkCore;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Data
{
    /// <summary>
    /// Database context for Manufacturer-Phone application using Entity Framework Core.
    /// </summary>
    public class ManufacturerPhoneContext : DbContext
    {
        /// <summary>
        /// Gets or sets the manufacturers DbSet.
        /// </summary>
        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        /// <summary>
        /// Gets or sets the phones DbSet.
        /// </summary>
        public DbSet<Phone> Phones { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerPhoneContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ManufacturerPhoneContext(DbContextOptions<ManufacturerPhoneContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the model for the context using Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Manufacturer entity
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturers");

                entity.HasKey(m => m.Id);

                entity.Property(m => m.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd();

                entity.Property(m => m.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(m => m.Address)
                    .HasColumnName("Address")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(m => m.IsAChildCompany)
                    .HasColumnName("IsAChildCompany")
                    .HasDefaultValue(false);

                // Configure index on Name for better performance
                entity.HasIndex(m => m.Name)
                    .HasDatabaseName("IX_Manufacturers_Name");
            });

            // Configure Phone entity
            modelBuilder.Entity<Phone>(entity =>
            {
                entity.ToTable("Phones");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd();

                entity.Property(p => p.Model)
                    .HasColumnName("Model")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.SerialNumber)
                    .HasColumnName("SerialNumber")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(p => p.PhoneType)
                    .HasColumnName("PhoneType")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(p => p.ManufacturerId)
                    .HasColumnName("ManufacturerId")
                    .IsRequired();

                // Configure relationship
                entity.HasOne(p => p.Manufacturer)
                    .WithMany(m => m.Phones)
                    .HasForeignKey(p => p.ManufacturerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Phones_Manufacturers");

                // Configure unique constraint on SerialNumber
                entity.HasIndex(p => p.SerialNumber)
                    .IsUnique()
                    .HasDatabaseName("IX_Phones_SerialNumber_Unique");

                // Configure index on ManufacturerId for better performance
                entity.HasIndex(p => p.ManufacturerId)
                    .HasDatabaseName("IX_Phones_ManufacturerId");
            });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Data
{
    /// <summary>
    /// Database context implementing Table-Per-Hierarchy (TPH) inheritance strategy.
    /// All phone types are stored in a single table with a discriminator column.
    /// </summary>
    public class TphPhoneContext : BasePhoneContext
    {
        /// <summary>
        /// Initializes a new instance of the TphPhoneContext class.
        /// </summary>
        public TphPhoneContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TphPhoneContext class with options.
        /// </summary>
        /// <param name="options">The options for this context</param>
        public TphPhoneContext(DbContextOptions<TphPhoneContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the database connection for SQLite.
        /// </summary>
        /// <param name="optionsBuilder">The options builder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=phones_tph.db");
            }
        }

        /// <summary>
        /// Configures the TPH inheritance strategy using a discriminator column.
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected override void ConfigureInheritanceStrategy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phone>()
                .HasDiscriminator<string>("PhoneType")
                .HasValue<Smartphone>("Smartphone")
                .HasValue<FeaturePhone>("FeaturePhone")
                .HasValue<GamingPhone>("GamingPhone");
        }
    }
}
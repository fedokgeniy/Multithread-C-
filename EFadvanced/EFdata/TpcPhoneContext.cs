using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Data
{
    /// <summary>
    /// Database context implementing Table-Per-Concrete-Type (TPC) inheritance strategy.
    /// Each concrete type has its own complete table with all properties.
    /// </summary>
    public class TpcPhoneContext : BasePhoneContext
    {
        /// <summary>
        /// Initializes a new instance of the TpcPhoneContext class.
        /// </summary>
        public TpcPhoneContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TpcPhoneContext class with options.
        /// </summary>
        /// <param name="options">The options for this context</param>
        public TpcPhoneContext(DbContextOptions<TpcPhoneContext> options) : base(options)
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
                optionsBuilder.UseSqlite("Data Source=phones_tpc.db");
            }
        }

        /// <summary>
        /// Configures the TPC inheritance strategy with complete tables for each concrete type.
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected override void ConfigureInheritanceStrategy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phone>().UseTpcMappingStrategy();
            modelBuilder.Entity<Smartphone>().ToTable("Smartphones");
            modelBuilder.Entity<FeaturePhone>().ToTable("FeaturePhones");
            modelBuilder.Entity<GamingPhone>().ToTable("GamingPhones");
        }
    }
}
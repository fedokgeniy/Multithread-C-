using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Data
{
    /// <summary>
    /// Database context implementing Table-Per-Type (TPT) inheritance strategy.
    /// Each type in the hierarchy has its own table with foreign key relationships.
    /// </summary>
    public class TptPhoneContext : BasePhoneContext
    {
        /// <summary>
        /// Initializes a new instance of the TptPhoneContext class.
        /// </summary>
        public TptPhoneContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TptPhoneContext class with options.
        /// </summary>
        /// <param name="options">The options for this context</param>
        public TptPhoneContext(DbContextOptions<TptPhoneContext> options) : base(options)
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
                optionsBuilder.UseSqlite("Data Source=phones_tpt.db");
            }
        }

        /// <summary>
        /// Configures the TPT inheritance strategy with separate tables for each type.
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected override void ConfigureInheritanceStrategy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phone>().ToTable("Phones");
            modelBuilder.Entity<Smartphone>().ToTable("Smartphones");
            modelBuilder.Entity<FeaturePhone>().ToTable("FeaturePhones");
            modelBuilder.Entity<GamingPhone>().ToTable("GamingPhones");
        }
    }
}
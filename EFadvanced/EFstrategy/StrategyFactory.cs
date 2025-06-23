using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Data;
using PhoneInheritanceDemo.Services;

namespace PhoneInheritanceDemo.Strategies
{
    /// <summary>
    /// Factory class for creating phone service instances with different inheritance strategies.
    /// Provides a centralized way to create and configure services for TPH, TPT, and TPC strategies.
    /// </summary>
    public static class StrategyFactory
    {
        /// <summary>
        /// Creates a phone service using Table-Per-Hierarchy (TPH) strategy.
        /// All phone types are stored in a single table with a discriminator column.
        /// </summary>
        /// <returns>A configured TPH phone service instance</returns>
        public static IPhoneService CreateTphService()
        {
            var options = new DbContextOptionsBuilder<TphPhoneContext>()
                .UseSqlite("Data Source=phones_tph.db")
                .Options;

            var context = new TphPhoneContext(options);
            return new TphPhoneService(context);
        }

        /// <summary>
        /// Creates a phone service using Table-Per-Type (TPT) strategy.
        /// Each type in the hierarchy has its own table with foreign key relationships.
        /// </summary>
        /// <returns>A configured TPT phone service instance</returns>
        public static IPhoneService CreateTptService()
        {
            var options = new DbContextOptionsBuilder<TptPhoneContext>()
                .UseSqlite("Data Source=phones_tpt.db")
                .Options;

            var context = new TptPhoneContext(options);
            return new TptPhoneService(context);
        }

        /// <summary>
        /// Creates a phone service using Table-Per-Concrete-Type (TPC) strategy.
        /// Each concrete type has its own complete table with all properties.
        /// </summary>
        /// <returns>A configured TPC phone service instance</returns>
        public static IPhoneService CreateTpcService()
        {
            var options = new DbContextOptionsBuilder<TpcPhoneContext>()
                .UseSqlite("Data Source=phones_tpc.db")
                .Options;

            var context = new TpcPhoneContext(options);
            return new TpcPhoneService(context);
        }
    }
}
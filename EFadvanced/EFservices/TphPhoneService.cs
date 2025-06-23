using PhoneInheritanceDemo.Data;

namespace PhoneInheritanceDemo.Services
{
    /// <summary>
    /// Phone service implementation for Table-Per-Hierarchy (TPH) inheritance strategy.
    /// Uses a single table to store all phone types with a discriminator column.
    /// </summary>
    public class TphPhoneService : BasePhoneService<TphPhoneContext>
    {
        /// <summary>
        /// Initializes a new instance of the TphPhoneService class.
        /// </summary>
        /// <param name="context">The TPH database context</param>
        public TphPhoneService(TphPhoneContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the name of the inheritance strategy.
        /// </summary>
        public override string StrategyName => "Table-Per-Hierarchy (TPH)";
    }
}
using PhoneInheritanceDemo.Data;

namespace PhoneInheritanceDemo.Services
{
    /// <summary>
    /// Phone service implementation for Table-Per-Type (TPT) inheritance strategy.
    /// Uses separate tables for each type in the inheritance hierarchy.
    /// </summary>
    public class TptPhoneService : BasePhoneService<TptPhoneContext>
    {
        /// <summary>
        /// Initializes a new instance of the TptPhoneService class.
        /// </summary>
        /// <param name="context">The TPT database context</param>
        public TptPhoneService(TptPhoneContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the name of the inheritance strategy.
        /// </summary>
        public override string StrategyName => "Table-Per-Type (TPT)";
    }
}
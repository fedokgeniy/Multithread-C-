using PhoneInheritanceDemo.Data;

namespace PhoneInheritanceDemo.Services
{
    /// <summary>
    /// Phone service implementation for Table-Per-Concrete-Type (TPC) inheritance strategy.
    /// Uses complete tables for each concrete type without base table.
    /// </summary>
    public class TpcPhoneService : BasePhoneService<TpcPhoneContext>
    {
        /// <summary>
        /// Initializes a new instance of the TpcPhoneService class.
        /// </summary>
        /// <param name="context">The TPC database context</param>
        public TpcPhoneService(TpcPhoneContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the name of the inheritance strategy.
        /// </summary>
        public override string StrategyName => "Table-Per-Concrete-Type (TPC)";
    }
}
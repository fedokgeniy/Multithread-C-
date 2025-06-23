using PhoneInheritanceDemo.Data;

namespace PhoneInheritanceDemo.Services;

/// <summary>
/// Phone service implementation using Table-Per-Concrete-Type (TPC) strategy.
/// </summary>
public class TpcPhoneService : BasePhoneService<TpcPhoneContext>
{
    /// <summary>
    /// Initializes a new instance of the TpcPhoneService class.
    /// </summary>
    /// <param name="context">The TPC database context.</param>
    public TpcPhoneService(TpcPhoneContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets the strategy name.
    /// </summary>
    /// <returns>The inheritance strategy name.</returns>
    public override string GetStrategyName()
    {
        return "Table-Per-Concrete-Type (TPC)";
    }
}
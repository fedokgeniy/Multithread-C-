using PhoneInheritanceDemo.Data;

namespace PhoneInheritanceDemo.Services;

/// <summary>
/// Phone service implementation using Table-Per-Hierarchy (TPH) strategy.
/// </summary>
public class TphPhoneService : BasePhoneService<TphPhoneContext>
{
    /// <summary>
    /// Initializes a new instance of the TphPhoneService class.
    /// </summary>
    /// <param name="context">The TPH database context.</param>
    public TphPhoneService(TphPhoneContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets the strategy name.
    /// </summary>
    /// <returns>The inheritance strategy name.</returns>
    public override string GetStrategyName()
    {
        return "Table-Per-Hierarchy (TPH)";
    }
}
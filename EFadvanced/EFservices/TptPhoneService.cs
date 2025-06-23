using PhoneInheritanceDemo.Data;

namespace PhoneInheritanceDemo.Services;

/// <summary>
/// Phone service implementation using Table-Per-Type (TPT) strategy.
/// </summary>
public class TptPhoneService : BasePhoneService<TptPhoneContext>
{
    /// <summary>
    /// Initializes a new instance of the TptPhoneService class.
    /// </summary>
    /// <param name="context">The TPT database context.</param>
    public TptPhoneService(TptPhoneContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets the strategy name.
    /// </summary>
    /// <returns>The inheritance strategy name.</returns>
    public override string GetStrategyName()
    {
        return "Table-Per-Type (TPT)";
    }
}
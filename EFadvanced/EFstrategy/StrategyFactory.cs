using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Data;
using PhoneInheritanceDemo.Services;

namespace PhoneInheritanceDemo.Strategies;

/// <summary>
/// Factory for creating different inheritance strategy implementations.
/// </summary>
public static class StrategyFactory
{
    /// <summary>
    /// Creates a phone service using the TPH (Table-Per-Hierarchy) strategy.
    /// </summary>
    /// <returns>A phone service using TPH strategy.</returns>
    public static IPhoneService CreateTphService()
    {
        var options = new DbContextOptionsBuilder<TphPhoneContext>()
            .UseInMemoryDatabase(databaseName: "TphPhoneDatabase")
            .Options;

        var context = new TphPhoneContext(options);
        return new TphPhoneService(context);
    }

    /// <summary>
    /// Creates a phone service using the TPT (Table-Per-Type) strategy.
    /// </summary>
    /// <returns>A phone service using TPT strategy.</returns>
    public static IPhoneService CreateTptService()
    {
        var options = new DbContextOptionsBuilder<TptPhoneContext>()
            .UseInMemoryDatabase(databaseName: "TptPhoneDatabase")
            .Options;

        var context = new TptPhoneContext(options);
        return new TptPhoneService(context);
    }

    /// <summary>
    /// Creates a phone service using the TPC (Table-Per-Concrete-Type) strategy.
    /// </summary>
    /// <returns>A phone service using TPC strategy.</returns>
    public static IPhoneService CreateTpcService()
    {
        var options = new DbContextOptionsBuilder<TpcPhoneContext>()
            .UseInMemoryDatabase(databaseName: "TpcPhoneDatabase")
            .Options;

        var context = new TpcPhoneContext(options);
        return new TpcPhoneService(context);
    }

    /// <summary>
    /// Gets all available strategy names.
    /// </summary>
    /// <returns>List of strategy names.</returns>
    public static List<string> GetAvailableStrategies()
    {
        return new List<string>
        {
            "TPH (Table-Per-Hierarchy)",
            "TPT (Table-Per-Type)",
            "TPC (Table-Per-Concrete-Type)"
        };
    }

    /// <summary>
    /// Creates a phone service based on strategy choice.
    /// </summary>
    /// <param name="strategyChoice">The strategy choice (1=TPH, 2=TPT, 3=TPC).</param>
    /// <returns>A phone service using the selected strategy.</returns>
    public static IPhoneService CreateServiceByChoice(int strategyChoice)
    {
        return strategyChoice switch
        {
            1 => CreateTphService(),
            2 => CreateTptService(),
            3 => CreateTpcService(),
            _ => throw new ArgumentException("Invalid strategy choice. Must be 1, 2, or 3.")
        };
    }
}
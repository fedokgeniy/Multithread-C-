using ConcurrencyAsynchrony;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Concurrent;
using System.Threading.Tasks;

[TestFixture]
public class DataSorterTests
{
    private ConcurrentDictionary<string, ConcurrentBag<string>> _fileData;
    private DataSorter _sorter;

    [SetUp]
    public void Setup()
    {
        _fileData = new ConcurrentDictionary<string, ConcurrentBag<string>>();
        _sorter = new DataSorter(_fileData);
    }

    [Test]
    public async Task StartAsync_ShouldSortDataPeriodically()
    {
        var testData = new ConcurrentBag<string> { "Z", "A", "M" };
        _fileData["test.txt"] = testData;

        await _sorter.StartAsync();
        await Task.Delay(AppConstants.SortingIntervalMs + 500);
        await _sorter.StopAsync();

        CollectionAssert.AreEqual(new[] { "A", "M", "Z" }, _fileData["test.txt"].OrderBy(x => x));
    }

    [Test]
    public void Dispose_ShouldStopSortingProcess()
    {
        _sorter.Dispose();
        Assert.ThrowsAsync<ObjectDisposedException>(async () => await _sorter.StartAsync());
    }

    [TearDown]
    public void Cleanup()
    {
        _sorter.Dispose();
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyAsynchrony
{
    /// <summary>
    /// Performs background data sorting in a thread-safe manner.
    /// Implements IDisposable interface for proper resource cleanup.
    /// </summary>
    public class DataSorter : IDisposable
    {
        private readonly ConcurrentDictionary<string, ConcurrentBag<string>> _fileData;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _sortingTask;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the DataSorter class.
        /// </summary>
        /// <param name="fileData">Thread-safe dictionary with data for sorting.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileData is null.</exception>
        public DataSorter(ConcurrentDictionary<string, ConcurrentBag<string>> fileData)
        {
            _fileData = fileData ?? throw new ArgumentNullException(nameof(fileData));
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Starts the sorting process in the background.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the object has been disposed.</exception>
        public async Task StartAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DataSorter));

            _sortingTask = SortDataPeriodicallyAsync(_cancellationTokenSource.Token);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Periodically performs data sorting.
        /// </summary>
        /// <param name="cancellationToken">Token for canceling the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SortDataPeriodicallyAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await PerformSortingAsync();
                    await Task.Delay(AppConstants.SortingIntervalMs, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in sorting process: {ex.Message}");
            }
        }

        /// <summary>
        /// Performs data sorting in files.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task PerformSortingAsync()
        {
            await Task.Run(() =>
            {
                foreach (var kvp in _fileData)
                {
                    var items = new List<string>();
                    while (kvp.Value.TryTake(out string item))
                    {
                        items.Add(item);
                    }

                    items.Sort();

                    foreach (var sortedItem in items)
                    {
                        kvp.Value.Add(sortedItem);
                    }
                }
            });
        }

        /// <summary>
        /// Stops the sorting process and waits for completion.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StopAsync()
        {
            if (_disposed || _cancellationTokenSource.IsCancellationRequested)
                return;

            _cancellationTokenSource.Cancel();

            if (_sortingTask != null)
            {
                try
                {
                    await _sortingTask;
                }
                catch (OperationCanceledException)
                {
                }
            }
        }

        /// <summary>
        /// Releases managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        /// <param name="disposing">Indicates whether managed resources are being disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _cancellationTokenSource?.Cancel();

                try
                {
                    _sortingTask?.Wait(TimeSpan.FromSeconds(5));
                }
                catch (AggregateException)
                {
                }

                _cancellationTokenSource?.Dispose();
                _disposed = true;
            }
        }
    }
}

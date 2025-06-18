using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcurrencyAsynchrony
{
    /// <summary>
    /// Responsible for displaying data and progress indicators in the console.
    /// Enhanced version with proper progress bar handling.
    /// </summary>
    public class DisplayManager
    {
        private readonly object _consoleLock = new();

        /// <summary>
        /// Shows a simple progress bar that doesn't interfere with other output.
        /// </summary>
        /// <param name="fileName">File name for progress tracking.</param>
        /// <param name="current">Current progress value.</param>
        /// <param name="total">Maximum progress value.</param>
        /// <param name="segments">Number of progress bar segments.</param>
        public void ShowProgressBar(string fileName, int current, int total, int segments = 10)
        {
            lock (_consoleLock)
            {
                int filled = Math.Min(segments, (int)Math.Round((double)current / total * segments));
                int empty = segments - filled;

                var progressBar = $"[{new string('■', filled)}{new string('─', empty)}]";
                var percentage = Math.Round((double)current / total * 100, 1);

                Console.WriteLine($"{fileName}: {progressBar} {current}/{total} ({percentage}%)");

                // Add a small delay to make progress visible
                if (current < total)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// Shows progress for all files at once (alternative approach).
        /// </summary>
        /// <param name="fileProgresses">Dictionary with file progress data.</param>
        public void ShowAllProgressBars(Dictionary<string, (int current, int total)> fileProgresses)
        {
            lock (_consoleLock)
            {
                Console.WriteLine("\n=== Reading Progress ===");
                foreach (var kvp in fileProgresses)
                {
                    var fileName = kvp.Key;
                    var (current, total) = kvp.Value;

                    int filled = Math.Min(10, (int)Math.Round((double)current / total * 10));
                    int empty = 10 - filled;

                    var progressBar = $"[{new string('■', filled)}{new string('─', empty)}]";
                    var percentage = Math.Round((double)current / total * 100, 1);

                    Console.WriteLine($"{fileName}: {progressBar} {current}/{total} ({percentage}%)");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Safely writes a message to the console.
        /// </summary>
        /// <param name="message">Message to display.</param>
        public void WriteLineThreadSafe(string message)
        {
            lock (_consoleLock)
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Safely writes text to the console without a line break.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void WriteThreadSafe(string text)
        {
            lock (_consoleLock)
            {
                Console.Write(text);
            }
        }

        /// <summary>
        /// Displays the contents of all files in a formatted way.
        /// </summary>
        /// <param name="fileData">Dictionary with file data.</param>
        public void DisplayFileContents(ConcurrentDictionary<string, ConcurrentBag<string>> fileData)
        {
            lock (_consoleLock)
            {
                Console.WriteLine("\n=== File Contents ===");
                foreach (var kvp in fileData)
                {
                    Console.WriteLine($"\n File: {kvp.Key}");
                    Console.WriteLine($"Objects count: {kvp.Value.Count}");

                    int itemCount = 0;
                    foreach (var entry in kvp.Value)
                    {
                        if (itemCount < 3)
                        {
                            Console.WriteLine($"{entry}");
                        }
                        else if (itemCount == 3)
                        {
                            Console.WriteLine($"... and {kvp.Value.Count - 3} more items");
                            break;
                        }
                        itemCount++;
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Displays detailed file contents.
        /// </summary>
        /// <param name="fileData">Dictionary with file data.</param>
        public void DisplayDetailedFileContents(ConcurrentDictionary<string, ConcurrentBag<string>> fileData)
        {
            lock (_consoleLock)
            {
                Console.WriteLine("\n=== Detailed File Contents ===");
                foreach (var kvp in fileData)
                {
                    Console.WriteLine($"\n File: {kvp.Key}");
                    Console.WriteLine("All objects:");

                    int itemNumber = 1;
                    foreach (var entry in kvp.Value)
                    {
                        Console.WriteLine($"{itemNumber:D2}. {entry}");
                        itemNumber++;
                    }
                    Console.WriteLine($"Total objects in {kvp.Key}: {kvp.Value.Count}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Asynchronously displays sorted data.
        /// </summary>
        /// <param name="fileData">Dictionary with file data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DisplaySortedDataAsync(ConcurrentDictionary<string, ConcurrentBag<string>> fileData)
        {
            await Task.Run(() =>
            {
                WriteLineThreadSafe("\n=== File contents after sorting ===");
                DisplayDetailedFileContents(fileData);
            });
        }

        /// <summary>
        /// Displays the application header.
        /// </summary>
        public void ShowApplicationHeader()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║        Parallelism and Asynchrony Demo            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        /// <summary>
        /// Displays a completion message for a stage.
        /// </summary>
        /// <param name="stageName">Name of the completed stage.</param>
        public void ShowStageCompletion(string stageName)
        {
            Console.WriteLine();
            Console.WriteLine($"{stageName} completed successfully!");
            Console.WriteLine();
        }

        /// <summary>
        /// Displays error information.
        /// </summary>
        /// <param name="ex">Exception to display.</param>
        public void ShowError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Critical error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner error: {ex.InnerException.Message}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Shows a section separator.
        /// </summary>
        /// <param name="title">Section title.</param>
        public void ShowSectionSeparator(string title)
        {
            Console.WriteLine();
            Console.WriteLine($"═══ {title} ═══");
            Console.WriteLine();
        }
    }
}
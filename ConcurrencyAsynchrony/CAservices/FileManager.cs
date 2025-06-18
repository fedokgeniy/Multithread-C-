using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurrencyAsynchrony
{
    /// <summary>
    /// Responsible for file reading operations with progress display.
    /// Simple version that works reliably with console output.
    /// </summary>
    public class FileManager
    {
        private readonly DisplayManager _displayManager;
        private readonly ConcurrentDictionary<string, ConcurrentBag<string>> _fileData;

        /// <summary>
        /// Initializes a new instance of the FileManager class.
        /// </summary>
        /// <param name="displayManager">Display manager for showing progress.</param>
        /// <param name="fileData">Thread-safe dictionary for storing file data.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the parameters is null.</exception>
        public FileManager(DisplayManager displayManager, ConcurrentDictionary<string, ConcurrentBag<string>> fileData)
        {
            _displayManager = displayManager ?? throw new ArgumentNullException(nameof(displayManager));
            _fileData = fileData ?? throw new ArgumentNullException(nameof(fileData));
        }

        /// <summary>
        /// Asynchronously performs parallel file reading with progress display.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ParallelReadFilesWithProgressBarAsync()
        {
            _displayManager.ShowSectionSeparator("Parallel File Reading with Progress");

            // Start all file reading tasks
            var tasks = AppConstants.FileNames.Select(ReadFileWithProgressAsync).ToArray();

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // Display final results
            _displayManager.ShowStageCompletion("File Reading");
            _displayManager.DisplayFileContents(_fileData);
        }

        /// <summary>
        /// Asynchronously reads a file with progress display.
        /// </summary>
        /// <param name="fileName">File name to read.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ReadFileWithProgressAsync(string fileName)
        {
            try
            {
                // Check if file exists
                if (!File.Exists(fileName))
                {
                    _displayManager.WriteLineThreadSafe($"Warning: File {fileName} not found. Skipping...");
                    return;
                }

                _displayManager.WriteLineThreadSafe($"Starting to read {fileName}...");

                var lines = await File.ReadAllLinesAsync(fileName);
                var totalLines = lines.Length;
                var bag = new ConcurrentBag<string>();

                // Process each line with progress update
                for (int i = 0; i < totalLines; i++)
                {
                    var line = lines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        bag.Add(line.Trim());
                    }

                    // Update progress bar
                    _displayManager.ShowProgressBar(fileName, i + 1, totalLines, 10);

                    // Add delay as per requirement (100ms per line)
                    await Task.Delay(AppConstants.ProgressDelayMs);
                }

                // Store the data
                _fileData[fileName] = bag;
                _displayManager.WriteLineThreadSafe($"Completed reading {fileName} - {bag.Count} objects loaded");
            }
            catch (Exception ex)
            {
                _displayManager.WriteLineThreadSafe($"Error reading {fileName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously reads files without progress display (for other operations).
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ReadFilesAsync()
        {
            _displayManager.ShowSectionSeparator("Reading Files");

            var tasks = AppConstants.FileNames.Select(ReadFileAsync).ToArray();
            await Task.WhenAll(tasks);

            _displayManager.WriteLineThreadSafe("All files read successfully");
        }

        /// <summary>
        /// Asynchronously reads a single file without progress display.
        /// </summary>
        /// <param name="fileName">File name to read.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ReadFileAsync(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    _displayManager.WriteLineThreadSafe($"Warning: File {fileName} not found. Skipping...");
                    return;
                }

                var lines = await File.ReadAllLinesAsync(fileName);
                var bag = new ConcurrentBag<string>();

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        bag.Add(line.Trim());
                    }
                }

                _fileData[fileName] = bag;
                _displayManager.WriteLineThreadSafe($"Loaded {fileName}: {bag.Count} objects");
            }
            catch (Exception ex)
            {
                _displayManager.WriteLineThreadSafe($"Error reading {fileName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates sample files for testing if they don't exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateSampleFilesIfNeededAsync()
        {
            foreach (var fileName in AppConstants.FileNames)
            {
                if (!File.Exists(fileName))
                {
                    _displayManager.WriteLineThreadSafe($"Creating sample file: {fileName}");
                    await CreateSampleFileAsync(fileName);
                }
            }
        }

        /// <summary>
        /// Creates a sample file with test data.
        /// </summary>
        /// <param name="fileName">File name to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task CreateSampleFileAsync(string fileName)
        {
            try
            {
                using var writer = new StreamWriter(fileName);

                // Write 10 sample lines
                for (int i = 1; i <= AppConstants.ObjectsPerFile; i++)
                {
                    await writer.WriteLineAsync($"Sample object {i} in {fileName}");
                }

                _displayManager.WriteLineThreadSafe($"Created {fileName} with {AppConstants.ObjectsPerFile} lines");
            }
            catch (Exception ex)
            {
                _displayManager.WriteLineThreadSafe($"Error creating {fileName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the current file data.
        /// </summary>
        /// <returns>Thread-safe dictionary with file data.</returns>
        public ConcurrentDictionary<string, ConcurrentBag<string>> GetFileData()
        {
            return _fileData;
        }
    }
}
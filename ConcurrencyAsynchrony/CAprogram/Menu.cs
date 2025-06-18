using CAmodels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcurrencyAsynchrony
{
    internal class Program
    {
        private static readonly ConcurrentDictionary<string, ConcurrentBag<string>> _fileData = new();
        private static readonly DisplayManager _displayManager = new();
        private static readonly FileManager _fileManager = new(_displayManager, _fileData);
        private static readonly List<MenuItem> menuItems = new()
        {
            new MenuItem
            {
                Key = "1",
                Description = "Generate 50 objects and write to 5 files",
                Action = async () => await Stage0_GenerateAndSaveAsync()
            },
            new MenuItem
            {
                Key = "2",
                Description = "Read files in parallel, display data by keys",
                Action = async () => await Stage1_ParallelReadAndPrintAsync()
            },
            new MenuItem
            {
                Key = "3",
                Description = "Start background sorting of collections by ID",
                Action = async () => await Stage2_StartBackgroundSortAsync()
            },
            new MenuItem
            {
                Key = "4",
                Description = "Show file reading progress",
                Action = async () => await Stage3_ShowProgressBarAsync()
            },
            new MenuItem
            {
                Key = "0",
                Description = "Exit application",
                Action = () => Environment.Exit(0)
            }
        };

        private static async Task Main(string[] args)
        {
            // Show application header once
            _displayManager.ShowApplicationHeader();

            while (true)
            {
                ShowMenu();

                var input = Console.ReadLine();

                // Fix for NullReferenceException - add proper validation
                var selected = menuItems.Find(m => m.Key == input);
                if (selected != null && selected.Action != null)
                {
                    try
                    {
                        Console.Clear();
                        _displayManager.ShowSectionSeparator($"Executing: {selected.Description}");

                        var result = selected.Action.DynamicInvoke();
                        if (result is Task taskResult)
                        {
                            await taskResult;
                        }

                        Console.WriteLine("\n" + new string('═', 60));
                        Console.WriteLine("Operation completed successfully!");
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        _displayManager.ShowError(ex);
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║              STAGE MANAGEMENT MENU                ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            foreach (var item in menuItems)
            {
                Console.WriteLine($"  {item.Key}. {item.Description}");
            }

            Console.WriteLine();
            Console.WriteLine(new string('─', 54));
            Console.Write("Select an action (0-4): ");
        }

        private static async Task Stage0_GenerateAndSaveAsync()
        {
            try
            {
                var generator = new ObjectGenerator();
                await generator.GenerateAndSaveObjectsAsync();
                _displayManager.ShowStageCompletion("Stage 0 - Object Generation");
            }
            catch (Exception ex)
            {
                _displayManager.ShowError(ex);
                // Create sample files if generation fails
                Console.WriteLine("Creating sample files for testing...");
                await _fileManager.CreateSampleFilesIfNeededAsync();
            }
        }

        private static async Task Stage1_ParallelReadAndPrintAsync()
        {
            // Clear existing data
            _fileData.Clear();

            // Ensure files exist
            await _fileManager.CreateSampleFilesIfNeededAsync();

            // Read files without progress bars
            await _fileManager.ReadFilesAsync();

            // Display results
            _displayManager.DisplayFileContents(_fileData);
            _displayManager.ShowStageCompletion("Stage 1 - Parallel File Reading");
        }

        private static async Task Stage2_StartBackgroundSortAsync()
        {
            // Ensure we have data to sort
            if (_fileData.IsEmpty)
            {
                Console.WriteLine("No data to sort. Reading files first...");
                await _fileManager.CreateSampleFilesIfNeededAsync();
                await _fileManager.ReadFilesAsync();
            }

            _displayManager.ShowSectionSeparator("Background Sorting");
            Console.WriteLine("Starting background sorting...");

            using var sorter = new DataSorter(_fileData);
            await sorter.StartAsync();

            // Let it sort for a few seconds
            Console.WriteLine("Sorting in progress... (5 seconds)");
            for (int i = 5; i >= 1; i--)
            {
                Console.WriteLine($"Time remaining: {i} seconds");
                await Task.Delay(1000);
            }

            await sorter.StopAsync();

            Console.WriteLine("\n Displaying sorted data:");
            await _displayManager.DisplaySortedDataAsync(_fileData);
            _displayManager.ShowStageCompletion("Stage 2 - Background Sorting");
        }

        private static async Task Stage3_ShowProgressBarAsync()
        {
            // Clear existing data
            _fileData.Clear();

            Console.WriteLine("Demonstrating file reading with progress bars...");
            Console.WriteLine("Each file will be read with 100ms delay per line for visibility.");
            Console.WriteLine("Multiple files will be processed in parallel.");
            Console.WriteLine();

            // Ensure files exist
            await _fileManager.CreateSampleFilesIfNeededAsync();

            // Read files with progress bars
            await _fileManager.ParallelReadFilesWithProgressBarAsync();

            _displayManager.ShowStageCompletion("Stage 3 - Progress Bar Demo");
        }
    }
}

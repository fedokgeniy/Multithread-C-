using System;

namespace ConcurrencyAsynchrony
{
    /// <summary>
    /// Contains application constants for configuration management.
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// Names of files for storing data.
        /// </summary>
        public static readonly string[] FileNames =
        {
            "file1.txt",
            "file2.txt",
            "file3.txt",
            "file4.txt",
            "file5.txt"
        };

        /// <summary>
        /// The total number of objects created.
        /// </summary>
        public const int TotalObjects = 50;

        /// <summary>
        /// Number of objects in each file.
        /// </summary>
        public const int ObjectsPerFile = 10;

        /// <summary>
        /// Number of phones to create.
        /// </summary>
        public const int PhonesCount = 25;

        /// <summary>
        /// Number of producers to create.
        /// </summary>
        public const int ManufacturersCount = 25;

        /// <summary>
        /// Delay for showing progress (in milliseconds).
        /// </summary>
        public const int ProgressDelayMs = 100;

        /// <summary>
        /// Sorting interval (in milliseconds).
        /// </summary>
        public const int SortingIntervalMs = 500;
    }
}
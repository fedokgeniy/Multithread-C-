using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace ManufacturerPhoneApp.Extensions
{
    /// <summary>
    /// Extension methods for SqlDataReader to simplify data access.
    /// </summary>
    public static class SqlDataReaderExtensions
    {
        /// <summary>
        /// Gets a string value from the reader or returns an empty string if null.
        /// </summary>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The string value or empty string if null.</returns>
        public static string GetStringOrEmpty(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        /// <summary>
        /// Gets an integer value from the reader.
        /// </summary>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The integer value.</returns>
        public static int GetInt32(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetInt32(ordinal);
        }

        /// <summary>
        /// Gets a boolean value from the reader.
        /// </summary>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The boolean value.</returns>
        public static bool GetBoolean(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetBoolean(ordinal);
        }

        /// <summary>
        /// Gets a nullable integer value from the reader.
        /// </summary>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The nullable integer value.</returns>
        public static int? GetNullableInt32(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
        }
    }
}

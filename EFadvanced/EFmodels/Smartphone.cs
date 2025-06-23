using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models
{
    /// <summary>
    /// Represents a smartphone with advanced features and capabilities.
    /// Inherits from the base Phone class and adds smartphone-specific properties.
    /// </summary>
    public class Smartphone : Phone
    {
        /// <summary>
        /// Gets or sets the operating system of the smartphone.
        /// </summary>
        [StringLength(50)]
        public string OperatingSystem { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the RAM size in GB.
        /// </summary>
        [Range(1, 64)]
        public int RamSize { get; set; }

        /// <summary>
        /// Gets or sets the storage size in GB.
        /// </summary>
        [Range(16, 2048)]
        public int StorageSize { get; set; }

        /// <summary>
        /// Gets or sets the camera resolution in megapixels.
        /// </summary>
        [Range(1, 200)]
        public int CameraResolution { get; set; }

        /// <summary>
        /// Gets or sets whether the smartphone supports 5G connectivity.
        /// </summary>
        public bool HasFiveG { get; set; }

        /// <summary>
        /// Returns detailed smartphone information including advanced features.
        /// </summary>
        /// <returns>A formatted string with smartphone specifications</returns>
        public override string GetPhoneInfo()
        {
            var baseInfo = base.GetPhoneInfo();
            return $"{baseInfo} | OS: {OperatingSystem}, RAM: {RamSize}GB, Storage: {StorageSize}GB, Camera: {CameraResolution}MP, 5G: {(HasFiveG ? "Yes" : "No")}";
        }
    }
}
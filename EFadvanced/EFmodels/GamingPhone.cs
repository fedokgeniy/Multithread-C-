using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models
{
    /// <summary>
    /// Represents a gaming phone optimized for gaming performance.
    /// Inherits from the base Phone class and adds gaming-specific properties.
    /// </summary>
    public class GamingPhone : Phone
    {
        /// <summary>
        /// Gets or sets the name of the GPU.
        /// </summary>
        [StringLength(100)]
        public string GpuName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the screen refresh rate in Hz.
        /// </summary>
        [Range(60, 240)]
        public int RefreshRate { get; set; }

        /// <summary>
        /// Gets or sets whether the phone has gaming triggers.
        /// </summary>
        public bool HasGamingTriggers { get; set; }

        /// <summary>
        /// Gets or sets the type of cooling system.
        /// </summary>
        [StringLength(100)]
        public string CoolingSystem { get; set; } = string.Empty;

        /// <summary>
        /// Returns gaming phone information including gaming-specific features.
        /// </summary>
        /// <returns>A formatted string with gaming phone specifications</returns>
        public override string GetPhoneInfo()
        {
            var baseInfo = base.GetPhoneInfo();
            return $"{baseInfo} | GPU: {GpuName}, Refresh Rate: {RefreshRate}Hz, Gaming Triggers: {(HasGamingTriggers ? "Yes" : "No")}, Cooling: {CoolingSystem}";
        }
    }
}
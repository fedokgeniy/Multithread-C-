using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models
{
    /// <summary>
    /// Represents a feature phone with basic functionality.
    /// Inherits from the base Phone class and adds feature phone-specific properties.
    /// </summary>
    public class FeaturePhone : Phone
    {
        /// <summary>
        /// Gets or sets whether the phone has a physical keyboard.
        /// </summary>
        public bool HasPhysicalKeyboard { get; set; }

        /// <summary>
        /// Gets or sets the SMS storage capacity.
        /// </summary>
        [Range(50, 1000)]
        public int SmsStorageCapacity { get; set; }

        /// <summary>
        /// Gets or sets whether the phone includes basic games.
        /// </summary>
        public bool HasBasicGames { get; set; }

        /// <summary>
        /// Returns feature phone information including basic features.
        /// </summary>
        /// <returns>A formatted string with feature phone specifications</returns>
        public override string GetPhoneInfo()
        {
            var baseInfo = base.GetPhoneInfo();
            return $"{baseInfo} | Physical Keyboard: {(HasPhysicalKeyboard ? "Yes" : "No")}, SMS Storage: {SmsStorageCapacity}, Games: {(HasBasicGames ? "Yes" : "No")}";
        }
    }
}
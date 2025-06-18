using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAmodels
{
    /// <summary>
    /// Represents a menu item for the user interface.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the key for the menu item (e.g., "1").
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the description of the menu action.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the action to execute when the menu item is selected.
        /// </summary>
        public Action Action { get; set; }
    }
}

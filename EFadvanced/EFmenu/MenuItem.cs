using System;

namespace PhoneInheritanceDemo.Menu
{
    /// <summary>
    /// Represents a menu item with a title and action.
    /// Used to build interactive console menus.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the display text for the menu item.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action to execute when the menu item is selected.
        /// </summary>
        public Func<Task> Action { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the MenuItem class.
        /// </summary>
        /// <param name="title">The display text for the menu item</param>
        /// <param name="action">The action to execute when selected</param>
        public MenuItem(string title, Func<Task> action)
        {
            Title = title;
            Action = action;
        }
    }
}
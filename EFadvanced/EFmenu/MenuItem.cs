namespace PhoneInheritanceDemo.Menu;

/// <summary>
/// Represents a menu item with text and action.
/// </summary>
public class MenuItem
{
    /// <summary>
    /// Gets or sets the display text for the menu item.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the action to execute when the menu item is selected.
    /// </summary>
    public Func<Task>? Action { get; set; }

    /// <summary>
    /// Gets or sets whether this menu item should exit the menu.
    /// </summary>
    public bool IsExit { get; set; }

    /// <summary>
    /// Initializes a new instance of the MenuItem class.
    /// </summary>
    /// <param name="text">The display text.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="isExit">Whether this item exits the menu.</param>
    public MenuItem(string text, Func<Task>? action = null, bool isExit = false)
    {
        Text = text;
        Action = action;
        IsExit = isExit;
    }
}
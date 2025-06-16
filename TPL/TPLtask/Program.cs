using System;
using TPLmodels;
using TPLservices;

class Program
{
    /// <summary>
    /// The main entry point for the application. Displays the menu and processes user input.
    /// </summary>
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU ===");
            foreach (var item in Menu.Items)
            {
                Console.WriteLine($"{item.Key}. {item.Description}");
            }
            Console.Write("Choose an option: ");
            var input = Console.ReadLine();

            var menuItem = Menu.Items.Find(i => i.Key == input);
            if (menuItem != null)
            {
                menuItem.Action();
            }
            else
            {
                Console.WriteLine("Incorrect input. Please try again.");
            }
        }
    }
}

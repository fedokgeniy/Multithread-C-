using MultithreadingData;

while (true)
{
    Console.WriteLine("Menu:");
    foreach (var item in Menu.MenuItems)
        Console.WriteLine($"{item.Key}. {item.Description}");

    Console.Write("Choose menu item: ");
    var choice = Console.ReadLine();

    var menuItem = Menu.MenuItems.FirstOrDefault(m => m.Key == choice);
    if (menuItem != null)
        menuItem.Action();
    else
        Console.WriteLine("Invalid item. Try again.");
}

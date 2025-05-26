using MultithreadingData;

while (true)
{
    Console.WriteLine("Меню:");
    foreach (var item in Menu.MenuItems)
        Console.WriteLine($"{item.Key}. {item.Description}");

    Console.Write("Выберите пункт меню: ");
    var choice = Console.ReadLine();

    var menuItem = Menu.MenuItems.FirstOrDefault(m => m.Key == choice);
    if (menuItem != null)
        menuItem.Action();
    else
        Console.WriteLine("Неверный выбор. Попробуйте снова.");
}

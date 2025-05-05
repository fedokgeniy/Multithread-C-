using SerializationServices;
using SerializationModels;
using SerializationData;

while (true)
{
    Console.WriteLine("\nМеню:");
    foreach (var item in Menu.MenuItems)
        Console.WriteLine($"{item.Key} - {item.Description}");

    Console.Write("Выбор: ");
    var input = Console.ReadLine();

    var selected = Menu.MenuItems.FirstOrDefault(m => m.Key == input);
    if (selected != null)
        selected.Action.Invoke();
    else
        Console.WriteLine("Неверный выбор. Попробуйте снова.");
}
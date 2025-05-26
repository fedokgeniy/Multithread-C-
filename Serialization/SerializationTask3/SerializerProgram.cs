using SerializationServices;
using SerializationModels;
using SerializationData;

while (true)
{
    Console.WriteLine("\nMenu:");
    foreach (var item in Menu.MenuItems)
        Console.WriteLine($"{item.Key} - {item.Description}");

    Console.Write("Selection: ");
    var input = Console.ReadLine();

    var selected = Menu.MenuItems.FirstOrDefault(m => m.Key == input);
    if (selected != null)
        selected.Action.Invoke();
    else
        Console.WriteLine("Wrong selection. Try again.");
}
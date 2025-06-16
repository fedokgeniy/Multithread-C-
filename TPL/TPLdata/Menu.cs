using System;
using System.Collections.Generic;
using TPLmodels;
using TPLservices;

/// <summary>
/// Provides the application menu and its actions.
/// </summary>
public static class Menu
{
    /// <summary>
    /// The list of menu items available in the application.
    /// </summary>
    public static readonly List<MenuItem> Items = new()
    {
        new MenuItem
        {
            Key = "1",
            Description = "Генерировать 20 телефонов и сериализовать в два файла (параллельно)",
            Action = () => PhoneDemoService.GenerateAndSerialize()
        },
        new MenuItem
        {
            Key = "2",
            Description = "Объединить два файла телефонов в третий",
            Action = () => PhoneDemoService.MergeFiles()
        },
        new MenuItem
        {
            Key = "3",
            Description = "Асинхронно прочитать итоговый файл телефонов и вывести",
            Action = () => PhoneDemoService.ReadAndPrintAsync().GetAwaiter().GetResult()
        },
        new MenuItem
        {
            Key = "0",
            Description = "Выход",
            Action = () => Environment.Exit(0)
        }
    };
}




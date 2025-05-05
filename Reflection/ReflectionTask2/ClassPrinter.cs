using System;
using System.Numerics;
using System.Reflection;
using ReflectionLibrary;

Assembly assembly = typeof(Phone).Assembly;

Console.WriteLine($"List of classes in the library: {assembly.GetName().Name}:\n");

foreach (Type type in assembly.GetTypes())
{
    Console.WriteLine($"Class: {type.Name}");
    Console.WriteLine("Property:");

    foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
    {
        string accessModifier = prop.GetMethod?.IsPublic == true ? "Public" : "Private";
        Console.WriteLine($"  - {accessModifier} property {prop.Name}");
    }

    Console.WriteLine();
}
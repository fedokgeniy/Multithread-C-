using System.Runtime.InteropServices;
using System.Reflection;
using ReflectionLibrary;

class Program
{
    static void Main()
    {

        try
        {
            Console.WriteLine("Insert path of the desired .dll (example: D:\\\\C# projects\\\\Multythread CSharp\\\\Reflection\\\\ReflectionLibrary\\\\bin\\\\Debug\\\\net9.0\\\\ReflectionLibrary.dll):");
            string DllName = Console.ReadLine();

            Assembly assembly = Assembly.LoadFrom(DllName);

            Console.WriteLine("Insert name of a class (with namespace, example: ReflectionLibrary.Phone):");
            string className = Console.ReadLine();

            Console.WriteLine("Insert name of a method (example: PrintObject):");
            string methodName = Console.ReadLine();

            Type type = Type.GetType(className);

            if (type == null)
            {
                type = assembly.GetType(className);
            }

            if (type == null)
            {
                Console.WriteLine("Class not found");
                return;
            }

            object instance = Activator.CreateInstance(type, true);

            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                Console.WriteLine("Method not found");
                return;
            }

            ParameterInfo[] parameters = method.GetParameters();
            object[] arg = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Console.WriteLine($"Insert {parameters[i].ParameterType.Name} argument:");
                string input = Console.ReadLine();
                arg[i] = Convert.ChangeType(Console.ReadLine(), parameters[i].ParameterType);
            }

            object result = method.Invoke(instance, arg);
            if (result != null)
            {
                Console.WriteLine($"Result: {result}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
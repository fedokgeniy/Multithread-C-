using System;
using System.Reflection;
using ReflectionLibrary;
class Program
{
    private const string MethodName = "Create";
    private const string PhoneClassName = "ReflectionLibrary.Phone";
    private const string ManufacturerClassName = "ReflectionLibrary.Manufacturer";
    

    static void Main()
    {
        try
        {
            Console.WriteLine("Insert path of the desired .dll (example: D:\\\\C# projects\\\\Multythread CSharp\\\\Reflection\\\\ReflectionLibrary\\\\bin\\\\Debug\\\\net9.0\\\\ReflectionLibrary.dll):");
            string DllName = Console.ReadLine();

            Assembly assembly = Assembly.LoadFrom(DllName);

            Type phoneType = assembly.GetType(PhoneClassName);
            Type manufacturerType = assembly.GetType(ManufacturerClassName);

            if (phoneType == null || manufacturerType == null)
            {
                throw new Exception("Classes are not found in assembly!");
            }

            MethodInfo createPhoneMethod = phoneType.GetMethod(MethodName)!;
            object phoneTypeEnum = Enum.Parse(typeof(PhoneTypeEnum), "BudgetPhone");
            object[] parameters = { 1, "Nokia 3310", "123456789", phoneTypeEnum };
            object phoneInstance = createPhoneMethod.Invoke(null, parameters);

            MethodInfo createManufacturerMethod = manufacturerType.GetMethod(MethodName)!;
            object[] manufacturerParams = { "Nokia", "Helsinki, Finland", false };
            object manufacturerInstance = createManufacturerMethod.Invoke(null, manufacturerParams);

            if (phoneInstance == null || manufacturerInstance == null)
            {
                throw new Exception("Creation of instances was unsuccessfull!");
            }
            phoneType.GetMethod("PrintObject")?.Invoke(phoneInstance, null);
            manufacturerType.GetMethod("PrintObject")?.Invoke(manufacturerInstance, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error {ex.Message}");
        }
    }
}
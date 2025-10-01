using System.Reflection;
using System.Runtime.CompilerServices;

// In C# è possibile fare uso di "reflection" per verificare a runtime quale sia
// la struttura di un tipo (classe, struct, ecc...) o di un assembly.

void PrintDefinitionOf(Type type)
{
    MemberInfo[] members = type
        .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

    Console.WriteLine($"Members of the type {type.Name}:");

    foreach (MemberInfo member in members)
    {
        bool isGenerated = member.GetCustomAttribute<CompilerGeneratedAttribute>() != null;

        Console.Write($"- {member.MemberType}: {member.Name}");
        if (isGenerated)
            Console.Write(" (generated)");

        Console.WriteLine();
    }
}

Type myClassType = typeof(MyClass);
PrintDefinitionOf(myClassType);

// Output:
// Members of the type MyClass:
// - Method: get_MyStaticProperty (generated)
// - Method: set_MyStaticProperty (generated)
// - Method: get_MyProperty (generated)
// - Method: set_MyProperty (generated)
// - Method: get_MyProtectedProperty (generated)
// - Method: set_MyProtectedProperty (generated)
// - Constructor: .ctor
// - Property: MyStaticProperty
// - Property: MyProperty
// - Property: MyProtectedProperty
// - Field: MyField
// - Field: _myPrivateField
// - Field: <MyProperty>k__BackingField (generated)
// - Field: <MyProtectedProperty>k__BackingField (generated)
// - Field: <MyStaticProperty>k__BackingField (generated)
// - Field: MyConst



// È possibile fare uso di reflection anche per leggere i valori a runtime,
// bypassando eventuali controlli sui modificatori di accessibilità.

void PrintValuesOf(object obj)
{
    Type type = obj.GetType();

    MemberInfo[] members = type
        .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

    Console.WriteLine($"Values in object of type {type.Name}:");

    foreach (MemberInfo member in members)
    {
        if (member is PropertyInfo property)
        {
            Console.WriteLine($"- {property.Name}: {property.GetValue(obj)}");
        }
        else if (member is FieldInfo field)
        {
            Console.WriteLine($"- {field.Name}: {field.GetValue(obj)}");
        }
    }
}

MyClass.MyStaticProperty = 123;
MyClass myObject = new("1", 2, 3f, 4.0);
PrintValuesOf(myObject);

// Output:
// Values in object of type MyClass:
// - MyStaticProperty: 123
// - MyProperty: 2
// - MyProtectedProperty: 4
// - MyField: 1
// - _myPrivateField: 3
// - <MyProperty>k__BackingField: 2
// - <MyProtectedProperty>k__BackingField: 4
// - <MyStaticProperty>k__BackingField: 123
// - MyConst: 42



public class MyClass
{
    public const int MyConst = 42;

    public static int MyStaticProperty { get; set; }

    public string MyField;

    private float _myPrivateField;

    public MyClass(string field, int property, float privateField, double privateProperty)
    {
        MyField = field;
        MyProperty = property;
        _myPrivateField = privateField;
        MyProtectedProperty = privateProperty;
    }

    public int MyProperty { get; set; }

    protected double MyProtectedProperty { get; set; }
}

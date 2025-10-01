
// In C# è possibile definire classi con questa sintassi:
class MyClass
{
    // Questo è un campo.
    private int _field;

    // I campi "readonly" possono essere assegnati solo dal costruttore.
    private readonly int _readonlyField;

    // Questa è una auto-property, ovvero una property con getter e setter
    // generati automaticamente, che rispettivamente leggono e assegnano un
    // campo privato nascosto generato automaticamente.
    public int AutoProperty { get; set; }

    // Questa è una property con getter e setter definiti esplicitamente.
    public int Property
    {
        get
        {
            return _field;
        }
        set
        {
            _field = value;
        }
    }

    // È supportata anche una sintassi più semplice se getter o setter possono
    // essere espressi con una singola espressione di codice.
    public int Property2
    {
        get => _field;
        set => _field = value;
    }

    // Le property possono anche avere solo un getter, 
    // ppure volendo solo un setter (raramente).
    public int GetterOnly
    {
        get => _readonlyField;
    }

    // Per le property con solo un getter che può essere espresso in una singola
    // espressione di codice è supportata anche una sintassi più semplice.
    public int GetterOnly2 => _readonlyField;

    // Questo è il costruttore della classe, viene utilizzato per inizializzare
    // la classe ed è invocabile con la parola chiave "new" di C#.
    public MyClass(int value)
    {
        _readonlyField = value;
    }

    // Questo è un metodo.
    public void PrintValue()
    {
        Console.WriteLine($"Il valore della property è: {Property}");
    }

    // È possibile definire un metodo "static" accessibile ovunque senza bisogno
    // di creare istanze della classe. È possibile anche definire property e
    // campi "static", che saranno accessibili indicando il nome della classe
    // piuttosto che una variabile del tipo della classe.
    public static void Hello()
    {
        Console.WriteLine("Hello!");
    }
}

// Il "costruttore primario" è un costruttore pubblico che non contiene codice.
// C# permette di definirlo con una sintassi più semplice rispetto a quella di
// un costruttore tradizionale:
class MyClass2(int value)
{
    public void PrintValue()
    {
        Console.WriteLine($"Il valore della property è: {value}");
    }
}



// Ereditarietà e polimorfismo sono supportati.

// Una classe "abstract" non permette la creazione di istanze.
public abstract class Animal
{
    // Un metodo o una property può essere "abstract" per indicare che non è
    // definito un effetto. Questo è possibile solo in classi astratte.
    public abstract string Family { get; }

    public abstract string Name { get; }

    public abstract void Talk();
}

public class Dog : Animal
{
    // "override" indica che si sta definendo l'effetto di un metodo o
    // di una property definita in una classe astratta.
    public override string Family => "Canidae";

    // Una auto-property con solo un getter si comporta come un campo readonly,
    // ovvero è assegnabile solamente dal costruttore della classe.
    public override string Name { get; }

    public Dog(string name)
    {
        Name = name;
    }

    public override void Talk()
    {
        Console.WriteLine("Bau bau!");
    }
}

public class Cat : Animal
{
    public override string Family => "Felidae";

    public override string Name { get; }

    public Cat(string name)
    {
        Name = name;
    }

    public override void Talk()
    {
        Console.WriteLine("Miao!");
    }
}



// Non è supportata l'ereditarietà multipla, ma per ovviare questo problema è
// possibile fare uso di interfacce.
// Una classe può ereditare da una sola classe padre, ma può implementare più
// di un'interfaccia.
public interface IAnimal
{
    string Family { get; }

    string Name { get; }

    void Talk();
}

public interface IJumping
{
    void Jump();
}

public class Human : Animal, IAnimal, IJumping
{
    public override string Family => "Hominidae";

    public override string Name { get; }

    public Human(string name)
    {
        Name = name;
    }

    public override void Talk()
    {
        Console.WriteLine("Ciao!");
    }

    public void Jump()
    {
        Console.WriteLine("Hop!");
    }
}




// Le "struct" sono equivalenti alle "class", ma non supportano ereditarietà.
// A differenza delle classi, le strutture risiedono sullo stack piuttosto che
// sulla heap.
// Possono implementare interfacce.

public struct MyStruct : IJumping
{
    public void Jump()
    {
        Console.WriteLine("Jump called.");
    }
}



// Le "ref struct" sono equivalenti alle "struct", ma vengono trasferite per
// riferimento piuttosto che per copia, introducendo quindi alcune limitazioni.
// Sono utilizzate solitamente in ambiti dove è necessaria massima performance.
// Possono implementare interfacce.

public ref struct MyRefStruct : IJumping
{
    public void Jump()
    {
        Console.WriteLine("Jump called.");
    }
}



// I "delegate" permettono di definire la firma di una funzione.
public delegate int MyDelegate(int param, string anotherParam);



// "enum" è equivalente ad una "struct" contenente solo costanti tutti di tipo intero.
public enum MyEnum
{
    Default, // implicitamente 0
    First = 10,
    Second = 20,
    Third, // implicitamente 21
    Fourth, // implicitamente 22
    Hundredth = 100,
}

// Equivalente approssimativamente a:
public struct MyEnum2
{
    public const int Default = 0;
    public const int First = 10;
    public const int Second = 20;
    public const int Third = 21;
    public const int Fourth = 22;
    public const int Hundredth = 100;
}

// "record" è equivalente ad una "class" di sole property assegnate al costruttore.
public record MyRecord(int Test, string Val);

// Equivalente approssimativamente a:
public class MyRecord2
{
    public int Test { get; }

    public string Val { get; }

    public MyRecord2(int test, string val)
    {
        Test = test;
        Val = val;
    }
}



// C# è un linguaggio ad alto livello che mette a disposizione "syntax sugar"
// per implementare funzionalità complesse con poco codice.
//
// Per esempio una property viene convertita in un campo più due metodi.
//
// Per vedere nel dettaglio come uno snippet di codice viene "de-sugared"
// consiglio di utilizzare lo strumento open source SharpLab:
// https://sharplab.io/

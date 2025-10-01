
// Le variabili devono essere dichiarate prima di essere lette o assegnate.
int num0;
double dec0;
string str0;
bool bool0;

// Le variabili possono essere assegnate (o riassegnate) solo con valori del
// tipo indicato alla dichiarazione.
num0 = 4567;
dec0 = 45.67;
str0 = "Ciao, mondo!";
bool0 = false;

// È possibile anche unire dichiarazione ed assegnazione iniziale in una singola
// riga di codice.
int num1 = 1234;
double dec1 = 12.34;
string str1 = "Hello, World!";
bool bool1 = false;

// È possibile assegnare null ad una variabile indicando ? dopo il tipo.
int? num2 = null;
double? dec2 = null;
string? str2 = null;
bool? bool2 = null;

// È possibile anche dichiarare costanti con il prefisso "const".
// Le costanti accettano solo valori noti in fase di compilazione, e non possono
// essere riassegnate.
const int SomeConstant = 1234;
const double AnotherConstant = 12.34;
const string MyString = "test";



// In C# esistono "value type" e "reference type".

// I principali "value type" sono:
// sbyte    da -128 a 127                                   intero a 8 bit con segno
// byte     da 0 a 255                                      intero a 8 bit senza segno
// short    da -32768 a 32767                               intero a 16 bit con segno
// ushort   da 0 a 65535                                    intero a 16 bit senza segno
// int      da -2147483648 a 2147483647                     intero a 32 bit con segno
// uint     da 0 to 4294967295                              intero a 32 bit senza segno
// long     da -9223372036854775808 a 9223372036854775807   intero a 64 bit con segno
// ulong    da 0 a 18446744073709551615                     intero a 64 bit senza segno
// nint     equivale a int sulle piattaforme a 32 bit, oppure a long su quelle a 64 bit
// nuint    equivale a uint sulle piattaforme a 32 bit, oppure a ulong su quelle a 64 bit
// float 	da ±1,5×10^−45 a ±3,4×10^38                     decimale con ~6-9 cifre, 32 bit
// double   da ±5,0×10^−324 a ±1,7×10^308                   decimale con ~15-17 cifre, 64 bit
// decimal 	da ±1.0x10^-28 a ±7,9228x10^28 	                decimale con ~28-29 cifre, 128 bit
// bool     false oppure true                               occupa 8 bit, ma solo 1 bit è utilizzato
// char     da U+0000 a U+FFFF                              carattere unicode UTF-16, 16 bit

// I principali "reference type" sono:
// string   stringhe immutabili di char
// object   oggetto che accetta qualsiasi valore
// dynamic  oggetto che accetta qualsiasi valore bypassando i controlli in fase di compilazione

// I "value type" risiedono sullo stack, mentre i "reference type" sulla heap.

// Tutti i tipi "struct" o "enum" sono "value type".
// I tipi "class", "interface", "record" e "delegate" sono "reference type".
// Gli array sono considerati "class", dunque sono "reference type".

// Per ulteriori informazioni:
// https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/value-types



// Gli operatori aritmetici in C# sono i seguenti:
// ++   incremento
// --   decremento
// +    addizione
// -    sottrazione
// *    moltiplicazione
// /    divisione
// %    resto

int val1 = (10 + 2) * 2;
int val2 = 100 + val1;

// Per ulteriori informazioni:
// https://learn.microsoft.com/dotnet/csharp/language-reference/operators/arithmetic-operators



// Il costrutto "if/else" permette di eseguire in modo condizionale una porzione
// di codice:
if (bool1)
{
    // eseguito se "bool1 è true"
}
else
{
    // eseguito se "bool1 non è true"
}

// È possibile fare confronti con gli operatori:
// ==   uguale a
// !=   diverso da
// <    minore di
// >    maggiore di
// <=   minore o uguale a
// >=   maggiore o uguale a

if (num0 > 10)
{
    // eseguito se "num0 è maggiore di 10"
}

// Per ulteriori informazioni:
// https://learn.microsoft.com/dotnet/csharp/language-reference/operators/comparison-operators



// Sono supportati anche i seguenti operatori logici, applicabili su valori booleani:
// &&   and
// ||   or
// !    not

// Ed i seguenti operatori, applicabili individualmente per ogni bit su valori interi:
// &    and bit per bit
// |    or bit per bit
// ^    xor bit per bit
// ~    not bit per bit

// Per ulteriori informazioni e gli altri operatori utilizzati meno frequentemente:
// https://learn.microsoft.com/dotnet/csharp/language-reference/operators/



// Il costrutto "switch" permette di confrontare un valore con una serie di pattern:
switch (MyString)
{
    case "test":
        Console.WriteLine("The string is 'test'.");
        break;

    case "prova":
        Console.WriteLine("La stringa è 'prova'.");
        break;

    case { Length: > 10 }:
        Console.WriteLine("La stringa ha una lunghezza maggiore di 10.");
        break;

    default:
        Console.WriteLine("La stringa non è né 'test' né 'prova', e la lunghezza è minore o uguale a 10.");
        break;
}

// È disponibile anche una sintassi più concisa, utilizzabile quando si fa un'assegnazione:
string message = MyString switch
{
    "test" => "The string is 'test'.",
    "prova" => "La stringa è 'prova'.",
    { Length: > 10 } => "La stringa ha una lunghezza maggiore di 10.",
    _ => "La stringa non è né 'test' né 'prova', e la lunghezza è minore o uguale a 10."
};

Console.WriteLine(message);



// Sono presenti 3 costrutti per gestire i cicli.

// Il ciclo "for" permette di eseguire un'operazione su un range di valori:
for (int i = 0; i < 10; i++)
{
    Console.WriteLine($"Valore i: {i}");
}

// Il ciclo "while" permette di eseguire un'operazione finché una condizione è vera:
bool hasMore = true;
while (hasMore)
{
    Console.WriteLine($"Iterazione con ciclo while.");
    hasMore = false;
}

// È anche possibile verificare la condizione solo al termine, tramite "do/while":
do
{
    Console.WriteLine($"Iterazione con ciclo do-while.");
    hasMore = false;
}
while (hasMore);

// Il ciclo foreach permette di iterare gli elementi di un valore "enumerable":
int[] fibo = [0, 1, 1, 2, 3, 5, 8, 13, 21, 34];
foreach (int val in fibo)
{
    Console.WriteLine($"Fibo val: {val}");
}

// In un ciclo è possibile usare la parola chiave "break" per interrompere immediatamente l'iterazione.
for (int i = 0; i < 10; i++)
{
    if (i == 5)
        break;

    Console.WriteLine($"Valore i con break: {i}");
}

// In un ciclo è possibile usare la parola chiave "continue" per saltare una singola iterazione.
for (int i = 0; i < 10; i++)
{
    if (i == 5)
        continue;

    Console.WriteLine($"Valore i con continue: {i}");
}

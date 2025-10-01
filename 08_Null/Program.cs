using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// C# è un linguaggio strongly-typed, quindi non è consentito assegnare ad una
// variabile valori di un tipo diverso rispetto al tipo dichiarato.

// Il controllo che il tipo assegnato coincida con il tipo dichiarato viene
// eseguito in fase di compilazione ed è garantito che se un programma compila,
// il valore in una variabile è di un tipo accettato dalla variabile.

string str0 = "hello"; // OK
string str1 = 123; // Error: cannot implicitly convert 'int' to 'string'.

// Anche i controlli di nullabilità sui "value type" vengono eseguiti in fase
// di compilazione, garantendo che se un programma compila, il valore in una
// variabile non nullabile non può essere null.

// C# distingue anche una variabile nullabile da una variabile non nullabile.
int num0 = null; // Error: cannot convert null to 'int' because it's a non-nullable value type.
int? num1 = null; // OK

// I controlli di nullabilità sui "reference type" invece sono stati aggiunti
// in una versione più recente di .NET, e sono limitati ad un warning per non
// rompere la compatibilità con le versioni precedenti di linguaggio e runtime.

string str3 = null; // Warning: converting null literal or possible null value to non-nullable type.
string? str4 = null; // OK



// Il compilatore fa il possibile per segnalare inconsistenze sulla
// nullabilità dei tipi...

// Questo metodo accetta un parametro di tipo string non null:
string Duplicate(string str)
{
    return str + str;
}

string? nullStr = null;
string example1 = Duplicate(nullStr); // Warning: possible null reference argument for parameter 'str' in `string Duplicate(string str)`.

string example2 = "";
if (nullStr != null)
{
    example2 = Duplicate(nullStr); // OK perché questo codice non viene mai eseguito se nullStr è null.
}

// ...ma non sempre con i "reference type" è in grado di farlo correttamente.

string? test = null;
bool isNull = test == null;
if (isNull)
{
    test = "not null";
}

// In questa riga test non può essere null, ma il compilatore segnala comunque:
string example3 = Duplicate(test); // Warning: possible null reference argument for parameter 'str' in `string Duplicate(string str)`.

// In questo caso è possibile zittire il warning:
string example4 = Duplicate(test!); // OK

// Usare il ! (chiamato operatore di "null-forgiving") disabilita i controlli
// sui null da parte del compilatore, causando potenziali problemi a runtime:
string? test2 = null;
string bad = test2!.Substring(1); // Nessun warning, eccezione a runtime.

// Nota dell'autore: personalmente consiglio di non utilizzare mai l'operatore
// di "null-forgiving" a meno che non si stia scrivendo del codice estremamente
// minimale e si abbia assoluta certezza non dovrà mai essere modificato.
// Una soluzione più affidabile per gestire questa casistica è per esempio:
string good = (test2 ?? throw new Exception("test2 is null")).Substring(1);

// Nota dell'autore: una versione leggermente meno sicura è la seguente, che
// interrompe l'esecuzione solo nella versione di debug se test2 è null, ma non
// effettua alcun controllo nella versione di release.
Debug.Assert(test2 != null);
string good2 = test2.Substring(1);



// In alcune situazioni più complesse il compilatore potrebbe non essere in
// grado di rilevare in autonomia se un valore può o meno essere null:
bool TryFindBad(IEnumerable<string> items, string startsWith, out string? found)
{
    foreach (string item in items)
    {
        if (item.StartsWith(startsWith))
        {
            // Se questo metodo restituisce true,
            // found è sicuramente diverso da null.
            found = item;
            return true;
        }
    }

    found = null;
    return false;
}

string[] items = ["banana", "tomato", "apple"];
if (TryFindBad(items, "tom", out string? tom))
{
    // Dato che TryFindBad(…) ha restituito true,
    // la variabile "tom" ha sicuramente valore non null,
    // ma il compilatore non lo sa...

    string example = Duplicate(tom); // Warning: possible null reference argument for parameter 'str' in `string Duplicate(string str)`.
}

// In tal caso è possibile aiutare il compilatore tramite attributi:
bool TryFindGood(IEnumerable<string> items, string startsWith, [NotNullWhen(true)] out string? found)
{
    foreach (string item in items)
    {
        if (item.StartsWith(startsWith))
        {
            // Se questo metodo restituisce true,
            // found è sicuramente diverso da null.
            found = item;
            return true;
        }
    }

    found = null;
    return false;
}

if (TryFindGood(items, "tom", out string? tom2))
{
    string example = Duplicate(tom2); // OK
}



// Purtroppo esistono anche situazioni in cui una variabile può assumere
// valore null senza che il compilatore se ne renda conto.

MyStruct hello = new();
string oops = hello.ThisIsNull.Substring(1); // Nessun warning, eccezione a runtime.

// È necessario fare attenzione, in particolare quando nel codice sono presenti
// operatori di "null-forgiving" e quando sono presenti "value type" (struct)
// che contengono "reference type" all'interno.

struct MyStruct
{
    public string ThisIsNull { get; set; }
}



// Per ulteriori informazioni:
// https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/nullable-reference-types

using System.Globalization;

// C# permette di aggiungere a classi già esistenti metodi addizionali, chiamati
// metodi di estensione.
//
// Le fluent-api di LINQ sono implementate come metodi di estensione di
// 'IEnumerable<T>'.
//
// Per ulteriori informazioni:
// https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/extension-methods

string str = "1234";

// Chiama il metodo 'ToIntOrZero()' come se fosse un metodo di 'str'.
// In realtà è un metodo di estensione definito sotto.
int num = str.ToIntOrZero(); // 1234


// Solitamente, il nome delle classi di estensione termina con "Extensions".
static class StringExtensions
{
    // Questo è un metodo di estensione, notare la parola chiave "this" di
    // fronte al tipo del primo parametro.
    public static int ToIntOrZero(this string str)
    {
        if (int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out int parsed))
            return parsed;

        return 0;
    }
}

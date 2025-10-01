
// Le eccezioni sono uno strumento fornito da C# per interrompere il flusso di
// esecuzione "normale" in caso di anomalie riscontrate a runtime.
//
// Dato che sono pensate per segnalare anomalie, oltre ad un messaggio che lo
// sviluppatore può indicare per descrivere il problema, includono le
// informazioni necessarie per identificarne agevolmente il punto di origine.
// Queste informazioni consistono nello "stack trace", ovvero una traccia dello
// stack nell'istante nel quale l'eccezione viene lanciata con "throw".

static class Program
{
    public static void Main()
    {
        Console.WriteLine("ESEMPIO 1:");

        try
        {
            WillFail();
        }
        catch (Exception ex)
        {
            string exceptionData = ex.ToString();

            // Output:
            // System.Exception: Qualcosa è andato storto.
            //    at Program.ThisMethodThrowsAnException() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 115
            //    at Program.WillFail() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 107
            //    at Program.Main() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 19
            Console.WriteLine(exceptionData);
        }

        Console.WriteLine();
        Console.WriteLine("ESEMPIO 2:");

        // Le eccezioni senza un "throw" non hanno stack trace.
        Exception test = ThisMethodReturnsAnException();

        // Output:
        // System.Exception: Qualcosa è andato storto.
        Console.WriteLine(test);

        Console.WriteLine();
        Console.WriteLine("ESEMPIO 3:");

        try
        {
            // Output:
            // Intercettata eccezione 1: Qualcosa è andato storto.
            RethrowBadExample();
        }
        catch (Exception ex)
        {
            // Output:
            // System.Exception: Qualcosa è andato storto.
            //    at Program.RethrowBadExample() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 139
            //    at Program.Main() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 50
            Console.WriteLine(ex.ToString());
        }

        Console.WriteLine();
        Console.WriteLine("ESEMPIO 4:");

        try
        {
            // Output:
            // Intercettata eccezione 2: Qualcosa è andato storto.
            RethrowGoodExample();
        }
        catch (Exception ex)
        {
            // Output:
            // System.Exception: Qualcosa è andato storto.
            //    at Program.ThisMethodThrowsAnException() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 115
            //    at Program.WillFail() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 107
            //    at Program.RethrowGoodExample() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 147
            //    at Program.Main() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 68
            Console.WriteLine(ex.ToString());
        }

        Console.WriteLine();
        Console.WriteLine("ESEMPIO 5:");

        try
        {
            // Output:
            // Intercettata eccezione 3: Qualcosa è andato storto.
            WrapExample();
        }
        catch (Exception ex)
        {
            // Output:
            // System.Exception: Rilevata anomalia.
            //  ---> System.Exception: Qualcosa è andato storto.
            //    at Program.ThisMethodThrowsAnException() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 115
            //    at Program.WillFail() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 107
            //    at Program.WrapExample() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 165
            //    --- End of inner exception stack trace ---
            //    at Program.WrapExample() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 176
            //    at Program.Main() in C:\src\dotnet\14b_ExceptionsCaveats\Program.cs:line 88
            Console.WriteLine(ex.ToString());
        }
    }

    static void WillFail()
    {
        ThisMethodThrowsAnException();
    }

    static void ThisMethodThrowsAnException()
    {
        Exception exception = ThisMethodReturnsAnException();

        // Lo stack trace viene valorizzato in corrispondenza del "throw".
        throw exception;
    }

    static Exception ThisMethodReturnsAnException()
    {
        // Questa riga costruisce un'eccezione senza valorizzare lo stack trace.
        return new Exception("Qualcosa è andato storto.");
    }

    static void RethrowBadExample()
    {
        try
        {
            WillFail();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Intercettata eccezione 1: {ex.Message}");

            // Lo stack trace viene valorizzato in corrispondenza del "throw",
            // se si fa "throw" di un'eccezione per la quale è già stato
            // valorizzato lo stack trace, la traccia dello stack del primo "throw"
            // viene perso e rimpiazzato con la traccia dello stack dell'ultimo.

            throw ex; // Warning: Re-throwing caught exception changes stack information.
        }
    }

    static void RethrowGoodExample()
    {
        try
        {
            WillFail();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Intercettata eccezione 2: {ex.Message}");

            // Usando solo "throw" senza indicare l'eccezione, viene lasciata
            // propagare intercettata dal blocco "catch" attuale.
            // In questo caso viene preservato lo stack trace originale.

            throw; // OK
        }
    }

    static void WrapExample()
    {
        try
        {
            WillFail();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Intercettata eccezione 3: {ex.Message}");

            // Volendo è anche possibile inscatolare l'eccezione originale
            // e lanciarne una nuova. In tal caso viene preservato lo stack
            // trace originale, ed in più verrà aggiunto lo stack trace
            // nel punto di codice dove è stato fatto il nuovo "throw".

            throw new Exception("Rilevata anomalia.", ex);
        }
    }
}

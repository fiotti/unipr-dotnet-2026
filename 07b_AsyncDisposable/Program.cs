
// È prevista anche l'interfaccia "IAsyncDisposable" equivalente a
// "IDisposable", che esegue asincronamente l'operazione di dispose.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

public static class Program
{
    // Se è presente un metodo di nome "Main" viene usato come punto di ingresso.
    public static async Task Main()
    {
        await Example1Async();
        await Example2Async();
        await Example3Async();
    }

    public static async Task Example1Async()
    {
        MyAsyncDisposableClass test = new();

        // La classe è disposable, quindi deve essere fatta la dispose quando
        // non si utilizza più la sua istanza.
        await test.DisposeAsync();
    }

    public static async Task Example2Async()
    {
        // È possibile automatizzare la dispose al termine di un nuovo blocco
        // con la seguente sintassi:
        await using (MyAsyncDisposableClass test = new())
        {
            // Al termine di questo blocco di codice, viene automaticamente
            // invocato il metodo "DisposeAsync" sulla variabile "test".
        }
    }

    public static async Task Example3Async()
    {
        // La seguente sintassi è equivalente all'esempio precedente:
        await using MyAsyncDisposableClass test = new();

        // Al termine di questo blocco di codice, viene automaticamente
        // invocato il metodo "DisposeAsync" sulla variabile "test".
    }
}

// Questa è un'implementazione minimale di IAsyncDisposable solo a scopo dimostrativo:
class MyAsyncDisposableClass : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        Console.WriteLine("Disposing");
    }
}

// Questa è un'implementazione corretta di IAsyncDisposable:
class BetterAsyncDisposable : IDisposable, IAsyncDisposable
{
    // Esempio risorsa disposable.
    private FileStream? _disposableResource = new("example.txt", FileMode.Open, FileAccess.Read);

    // Esempio risorsa disposable asincronamente.
    private MyAsyncDisposableClass? _asyncDisposableResource = new();

    // Esempio puntatore a risorsa non gestita.
    private nint _unmanagedResourceHandle = Marshal.AllocHGlobal(4096);

    // Esempio buffer di grandi dimensioni.
    private byte[]? _bigBuffer = new byte[0x1000000]; // 16MiB

    private bool _disposed;

    [MemberNotNull(nameof(_bigBuffer))]
    void EnsureNotDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        Debug.Assert(_disposableResource is not null);
        Debug.Assert(_asyncDisposableResource is not null);
        Debug.Assert(_bigBuffer is not null);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Invocare il metodo "Dispose" degli oggetti che sono stati
                // creati dall'interno di questa classe, e impostarli a null.
                if (_disposableResource is not null)
                {
                    _disposableResource.Dispose();
                    _disposableResource = null;
                }

                // Eseguire questa operazione anche per tutti gli oggetti
                // IAsyncDisposable che sono anche IDisposable.
                if (_asyncDisposableResource is IDisposable disposable)
                {
                    disposable.Dispose();
                    _asyncDisposableResource = null;
                }
            }

            // Rilasciare le eventuali risorse native, ovvero risorse
            // non gestite da .NET, per esempio quando da un programma scritto
            // in C# si fa uso di componenti scritti in C o C++. Questa è una
            // casistica relativamente rara.
            Marshal.FreeHGlobal(_unmanagedResourceHandle);

            // Per agevolare il lavoro del garbage collector, è
            // consigliato assegnare valore null a eventuali campi contenenti
            // oggetti di grandi dimensioni, come per esempio buffer.
            _bigBuffer = null;

            _disposed = true;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        // Fare asincronamente dispose di tutti gli oggetti che lo supportano.
        if (_asyncDisposableResource is not null)
        {
            await _asyncDisposableResource.DisposeAsync();
            _asyncDisposableResource = null;
        }

        // Fare sincronamente dispose per tutti gli altri.
        if (_disposableResource is IAsyncDisposable disposable)
        {
            await disposable.DisposeAsync();
            _disposableResource = null;
        }
        else if (_disposableResource is not null)
        {
            _disposableResource.Dispose();
            _disposableResource = null;
        }
    }

    // Se sono presenti risorse native (casistica relativamente rara), è
    // necessario aggiungere un finalizzatore alla classe; se non sono presenti
    // risorse native non è necessario aggiungere un finalizzatore.
    // Il finalizzatore deve generalmente contenere solo una chiamata al metodo
    // "Dispose" e nient'altro, per rilasciare le risorse native nel caso in cui
    // lo sviluppatore si sia dimenticato la "using" o di chiamare "Dispose".
    ~BetterAsyncDisposable()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }
}



// Per ulteriori informazioni su IAsyncDisposable:
// https://learn.microsoft.com/dotnet/standard/garbage-collection/implementing-disposeasync

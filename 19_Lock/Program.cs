
// C# è un linguaggio multi-thread, è possibile lanciare nuovi thread con
// la sintassi seguente.

List<Thread> threads = [];

for (int i = 0; i < 5; i++)
{
    Thread thread = new(() =>
    {
        Console.WriteLine($"Ciao, sono il thread #{Environment.CurrentManagedThreadId}");
    });

    thread.Start();

    threads.Add(thread);
}

foreach (Thread thread in threads)
{
    thread.Join();
}

// Output:
// Ciao, sono il thread #11
// Ciao, sono il thread #10
// Ciao, sono il thread #13
// Ciao, sono il thread #12
// Ciao, sono il thread #14



// Per come le CPU moderne sono costruite, se più thread vanno ad operare su
// una singola variabile condivisa, non è garantito che il risultato finale sia
// equivalente al risultato se eseguito da un singolo thread.

int counter = 0;
int sharedCounter = 0;

for (int i = 0; i < 100000; i++)
{
    counter++;
}

List<Thread> threads2 = [];

for (int i = 0; i < 100; i++)
{
    Thread thread = new(() =>
    {
        for (int i = 0; i < 1000; i++)
        {
            sharedCounter++;
        }
    });

    thread.Start();

    threads2.Add(thread);
}

foreach (Thread thread in threads2)
{
    thread.Join();
}

Console.WriteLine($"Single thread: {counter}");
Console.WriteLine($"Multi-thread: {sharedCounter}");

// Output:
// Single thread: 100000
// Multi-thread: 98598



// Per coordinare l'accesso da parte di più thread ad una variabile condivisa,
// è possibile usare la parola chiave "lock".
//
// Un blocco di codice in una sezione "lock" può essere eseguito da al più un
// thread per volta, ed il compilatore inserisce automaticamente le istruzioni
// necessarie per garantire che tutte le modifiche effettuate da un thread
// all'interno della sezione "lock" siano visibili a tutti gli altri thread nel
// momento in cui entrano nella sezione "lock".

Lock guard = new();
int sharedCounter2 = 0;

List<Thread> threads3 = [];

for (int i = 0; i < 100; i++)
{
    Thread thread = new(() =>
    {
        for (int i = 0; i < 1000; i++)
        {
            lock (guard)
            {
                sharedCounter2++;
            }
        }
    });

    thread.Start();

    threads3.Add(thread);
}

foreach (Thread thread in threads3)
{
    thread.Join();
}

Console.WriteLine($"Multi-thread con lock: {sharedCounter2}");

// Output:
// Multi-thread con lock: 100000



// Lo stesso limite si applica ai task, che pur non essendo thread, possono
// essere eseguiti in parallelo, andando ad eseguire modifiche concorrenziali
// su variabili condivise.

int sharedCounter3 = 0;

List<Task> tasks = [];

for (int i = 0; i < 100; i++)
{
    Task task = Task.Run(async () =>
    {
        for (int i = 0; i < 1000; i++)
        {
            sharedCounter3++;
        }
    });

    tasks.Add(task);
}

await Task.WhenAll(tasks);

Console.WriteLine($"Multi-task: {sharedCounter3}");

// Output:
// Multi-task: 42342



// In un contesto "async" come quello dei task, usare un lock è sconsigliato:
// https://learn.microsoft.com/aspnet/core/fundamentals/best-practices#avoid-blocking-calls
//
// È possibile risolvere questo problema con un semaforo.

SemaphoreSlim semaphore = new(1);
int sharedCounter4 = 0;

List<Task> tasks2 = [];

for (int i = 0; i < 100; i++)
{
    Task task = Task.Run(async () =>
    {
        for (int i = 0; i < 1000; i++)
        {
            await semaphore.WaitAsync();
            try
            {
                sharedCounter4++;
            }
            finally
            {
                semaphore.Release();
            }
        }
    });

    tasks2.Add(task);
}

await Task.WhenAll(tasks2);

Console.WriteLine($"Multi-task con semaforo: {sharedCounter4}");

// Output:
// Multi-task con semaforo: 100000



// Sono presenti anche altri costrutti per coordinare l'accesso a risorse
// contese tra più task o thread, in particolare quelli esposti dalla classe
// System.Threading.Interlocked, e dalle altre class in System.Threading.
//
// Per ulteriori informazioni:
// https://learn.microsoft.com/dotnet/api/system.threading.interlocked
// https://learn.microsoft.com/dotnet/standard/threading/threading-objects-and-features


// Il linguaggio C# prevede l'utilizzo di stream per interagire con flussi di
// byte, come per esempio socket o file.

using System.Diagnostics;

void LowLevelFileStreamExample()
{
    // Apre uno stream in lettura sul file indicato.
    // (Ricordarsi "using" per chiudere automaticamente il file al termine.)
    using FileStream file = File.OpenRead("users.csv");

    // Legge un singolo byte dal file.
    int b = file.ReadByte();

    // Primo byte: 49 (char '1')
    Console.WriteLine($"Primo byte: {b} (char '{(char)b}')");

    // Posizione nel file: 1
    Console.WriteLine($"Posizione nel file: {file.Position}");

    // Alloca su stack un buffer di 4096 byte.
    Span<byte> buffer = stackalloc byte[4096];

    // Legge fino a 4096 byte e restituisce il numero di byte letti.
    int bytesRead = file.Read(buffer);

    // Altri bytes letti: 29
    Console.WriteLine($"Altri bytes letti: {bytesRead}");

    if (bytesRead > 0)
    {
        // Secondo byte: 44 (char ',')
        Console.WriteLine($"Secondo byte: {buffer[0]} (char '{(char)buffer[0]}')");
    }

    // Posizione nel file: 30
    Console.WriteLine($"Posizione nel file: {file.Position}");
}

LowLevelFileStreamExample();



// Per semplificare la lettura di dati strutturati da uno stream,
// è disponibile la classe StreamReader.

void HighLevelFileStreamExample()
{
    using FileStream file = File.OpenRead("users.csv");

    // Gli "stream reader" forniscono metodi ad alto livello per leggere dati dagli stream.
    using StreamReader reader = new(file, leaveOpen: true);

    // Legge una riga di testo dallo stream.
    string? firstLine = reader.ReadLine();

    // Prima riga: 1,Pippo
    Console.WriteLine($"Prima riga: {firstLine}");

    // Legge un'altra riga di testo dallo stream.
    string? secondLine = reader.ReadLine();

    // Seconda riga: 2,Pluto
    Console.WriteLine($"Seconda riga: {secondLine}");

    // Legge il resto dello stream.
    string rest = reader.ReadToEnd();

    // Resto: 3,Paperino
    //   <- qui c'è un ritorno a capo
    Console.WriteLine($"Resto: {rest}");
}

HighLevelFileStreamExample();



// È possibile e consigliato usare "async/await" quando si interagisce con
// gli stream in C#.

async Task HighLevelFileStreamExampleAsync(CancellationToken cancellationToken = default)
{
    await using FileStream file = File.OpenRead("users.csv");

    // Gli "stream reader" forniscono metodi ad alto livello per leggere dati dagli stream.
    using StreamReader reader = new(file, leaveOpen: true);

    // Legge una riga di testo dallo stream.
    string? firstLine = await reader.ReadLineAsync(cancellationToken);

    // Prima riga: 1,Pippo
    Console.WriteLine($"Prima riga: {firstLine}");

    // Legge un'altra riga di testo dallo stream.
    string? secondLine = await reader.ReadLineAsync(cancellationToken);

    // Seconda riga: 2,Pluto
    Console.WriteLine($"Seconda riga: {secondLine}");

    // Legge il resto dello stream.
    string rest = await reader.ReadToEndAsync(cancellationToken);

    // Resto: 3,Paperino
    //   <- qui c'è un ritorno a capo
    Console.WriteLine($"Resto: {rest}");
}

await HighLevelFileStreamExampleAsync(CancellationToken.None);



// Gli stream sono bi-direzionali, ovvero permettono non solo di leggere bytes
// da un flusso, ma anche di scrivervi.

async Task WriteToFileExampleAsync(CancellationToken cancellationToken = default)
{
    // Apre un file in lettura e scrittura, rimpiazzandolo se esiste già.
    await using FileStream file = File.Open("~world.txt", FileMode.Create, FileAccess.ReadWrite);

    // Gli "stream writer" forniscono metodi ad alto livello per scrivere dati sugli stream.
    await using (StreamWriter writer = new(file, leaveOpen: true))
    {
        // Scrive una riga di testo sul file.
        await writer.WriteLineAsync("Hello, World!");

        await writer.FlushAsync(cancellationToken);
    }

    // Forza un flush del buffer di scrittura.
    await file.FlushAsync(cancellationToken);

    // Riporta lo stream al primo byte del file.
    file.Position = 0;

    using StreamReader reader = new(file);
    
    // Legge dal file la riga appena scritta.
    string? line = await reader.ReadLineAsync(cancellationToken);

    // Si assicura che coincida.
    Debug.Assert(line == "Hello, World!");
}

await WriteToFileExampleAsync();

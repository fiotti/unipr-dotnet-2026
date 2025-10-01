using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Durante lo il ciclo di vita di un'applicazione possono emergere bug o
// comportamenti non previsti. Questo può succedere a causa di aggiornamenti
// di componenti esterni, o per nuove pattern di utilizzo diverse da quelle
// ipotizzate in fase di analisi del software.
//
// Per agevolare l'identificazione di nuove problematiche, che talvolta
// emergono anche anni dopo un rilascio in produzione, è importante che
// il software emetta log che descrivano gli eventi interni.

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ProcessDataFile>();

using IHost host = builder.Build();
await host.RunAsync();

// Nota: grazie alla dependency-injection di Microsoft, il logger è
// accessibile ovunque tramite l'interfaccia ILogger<T>.
class ProcessDataFile(ILogger<ProcessDataFile> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // I messaggi informativi indicano cosa succede nel software.
            logger.LogInformation("Inizio elaborazione file...");

            if (!Environment.Is64BitProcess)
            {
                // I messaggi di warning indicano situazioni anomale.
                //
                // Solitamente indicano che l'operazione richiesta può comunque
                // essere portata a termine senza errori, ma che ciò
                // potrebbe non essere fatto in modo ottimale.
                logger.LogWarning("Questa applicazione dovrebbe essere eseguita in modalità a 64 bit.");
            }

            // I messaggi di debug indicano nel dettaglio lo stato delle
            // singole operazioni.
            //
            // Solitamente vengono disabilitati tramite file di configurazione
            // quando il software viene rilasciato in produzione.
            logger.LogDebug("Inizio ciclo sul file dei dati...");

            int rowNumber = 0;
            foreach (string row in File.ReadLines("data.txt"))
            {
                await ProcessRow(++rowNumber, row, stoppingToken);
            }

            logger.LogDebug("Fine ciclo sul file dei dati.");

            logger.LogInformation("Elaborazione file terminata senza errori.");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Elaborazione file interrotta.");
        }
        catch (Exception ex)
        {
            // I messaggi di errore critico indicano che il software ha
            // riscontrato un problema fatale e non è più in grado di
            // continuare l'esecuzione.
            //
            // Generalmente dopo un errore critico il software va in crash,
            // ovvero termina forzatamente e non procede con l'elaborazione.
            logger.LogCritical(ex, "Elaborazione file interrotta a causa di un errore non gestito.");
            throw;
        }
    }

    async Task ProcessRow(int rowNumber, string row, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogDebug("Elaborazione riga {RowNumber}: {Value}", rowNumber, row);

        if (string.IsNullOrWhiteSpace(row))
        {
            // I messaggi di errore indicano che non è stato possibile
            // completare correttamente l'operazione richiesta.
            //
            // Sarà necessario correggere manualmente il problema,
            // per esempio ripetendo la richiesta.
            logger.LogError("La riga {RowNumber} non contiene alcun valore, impossibile elaborare.", rowNumber);
            return;
        }

        // TODO: elaborare riga.
        await Task.Delay(500, cancellationToken);
    }
}

// Output:
// info: ProcessDataFile[0]
//       Inizio elaborazione file...
// dbug: ProcessDataFile[0]
//       Inizio ciclo sul file dei dati...
// dbug: ProcessDataFile[0]
//       Elaborazione riga 1: esempio riga 1
// info: Microsoft.Hosting.Lifetime[0]
//       Application started. Press Ctrl+C to shut down.
// info: Microsoft.Hosting.Lifetime[0]
//       Hosting environment: Production
// info: Microsoft.Hosting.Lifetime[0]
//       Content root path: C:\src\dotnet\15_Logging\bin\Debug\net8.0
// dbug: ProcessDataFile[0]
//       Elaborazione riga 2: riga 2
// dbug: ProcessDataFile[0]
//       Elaborazione riga 3: Riga 3.
// dbug: ProcessDataFile[0]
//       Elaborazione riga 4: riga 4...
// dbug: ProcessDataFile[0]
//       Elaborazione riga 5: pippo
// dbug: ProcessDataFile[0]
//       Elaborazione riga 6: pluto
// dbug: ProcessDataFile[0]
//       Elaborazione riga 7: paperino
// dbug: ProcessDataFile[0]
//       Elaborazione riga 8:
// fail: ProcessDataFile[0]
//       La riga 8 non contiene alcun valore, impossibile elaborare.
// dbug: ProcessDataFile[0]
//       Elaborazione riga 9: riga 9!
// dbug: ProcessDataFile[0]
//       Elaborazione riga 10: ultima riga
// dbug: ProcessDataFile[0]
//       Fine ciclo sul file dei dati.
// info: ProcessDataFile[0]
//       Elaborazione file terminata senza errori.
// info: Microsoft.Hosting.Lifetime[0]
//       Application is shutting down...



// Per ulteriori informazioni:
// https://learn.microsoft.com/dotnet/core/extensions/logging

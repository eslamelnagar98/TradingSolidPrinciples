using SD_PD_IDA_46.OCP;
using SD_PD_IDA_46.OCP.Interfaces;
using SD_PD_IDA_46.SRP;

namespace SD_PD_IDA_46.DIP;

// =====================================================================
// Dependency Inversion Principle (DIP)
//
// TWO rules:
//   1. High-level modules must NOT depend on low-level modules.
//      Both must depend on ABSTRACTIONS (interfaces).
//   2. Abstractions must NOT depend on details.
//      Details (concrete classes) must depend on abstractions.
//
// BEFORE DIP — TradeProcessor created its own dependencies:
//
//   var processor = new TradeProcessor(
//       new AdoNetTradeStorage(),        // ← high-level picks the concretion
//       new StreamTradeDataProvider(..), // ← tightly coupled, hard to swap/test
//       new TradeParser(),
//       new ConsoleLogger()
//   );
//
// AFTER DIP — TradeProcessor only knows about interfaces.
//   The CONTAINER decides which concrete class to inject.
//   Swapping ConsoleLogger → FileLogger = one line change HERE, zero changes in TradeProcessor.
// =====================================================================

public static class DipDemo
{
    public static void Run()
    {
        Console.WriteLine("--- Dependency Inversion Principle ---");
        Console.WriteLine();

        // ── Step 1: Build the container ──────────────────────────────
        // This is the ONLY place in the whole app that knows about concretions.
        // Everything else depends on interfaces only.
        IServiceCollection container = new ServiceCollection();

        container
            // Register ILogger → ConsoleLogger
            // To switch to FileLogger: change ONE line here, nothing else changes
            .Register<ILogger, ConsoleLogger>()

            // Register ITradeParser → TradeParser
            // TradeParser needs an ILogger in its constructor, so we use the factory overload
            .Register<ITradeParser, TradeParser>(() =>
            {
                var logger = container.Resolve<ILogger>();
                return new TradeParser(logger);
            })

            // Register ITradeDataProvider → ApiTradeDataProvider (needs url + logger)
            .Register<ITradeDataProvider, ApiTradeDataProvider>(() =>
            {
                var logger = container.Resolve<ILogger>();
                return new ApiTradeDataProvider(logger, "https://api.example.com/trades");
            })

            // Register ITradeStorage → CvsTradeStorage (needs filePath + logger)
            .Register<ITradeStorage, CvsTradeStorage>(() =>
            {
                var logger = container.Resolve<ILogger>();
                return new CvsTradeStorage("output_trades.csv", logger);
            });

        // ── Step 2: Resolve the high-level module ────────────────────
        // TradeProcessor is built by resolving its four interface dependencies.
        // TradeProcessor never calls `new` on anything — DIP achieved.
        var processor = new TradeProcessor(
            container.Resolve<ITradeStorage>(),
            container.Resolve<ITradeDataProvider>(),
            container.Resolve<ITradeParser>(),
            container.Resolve<ILogger>()
        );

        // ── Step 3: Run ───────────────────────────────────────────────
        processor.ProcessTrades();

        Console.WriteLine();

        // ── Step 4: Show how easy it is to SWAP a dependency ─────────
        // Business says: "log to a file instead of the console"
        // Under DIP: re-register one line, TradeProcessor source is untouched.
        Console.WriteLine("  [Swapping ILogger to FileLogger — zero changes in TradeProcessor]");

        IServiceCollection container2 = new ServiceCollection();
        container2
            .Register<ILogger, FileLogger>()          // ← the only change
            .Register<ITradeParser, TradeParser>(() =>
            {
                var logger = container2.Resolve<ILogger>();
                return new TradeParser(logger);
            })
            .Register<ITradeDataProvider, ApiTradeDataProvider>(() =>
            {
                var logger = container2.Resolve<ILogger>();
                return new ApiTradeDataProvider(logger, "https://api.example.com/trades");
            })
            .Register<ITradeStorage, CvsTradeStorage>(() =>
            {
                var logger = container2.Resolve<ILogger>();
                return new CvsTradeStorage("output_trades.csv", logger);
            });

        var processor2 = new TradeProcessor(
            container2.Resolve<ITradeStorage>(),
            container2.Resolve<ITradeDataProvider>(),
            container2.Resolve<ITradeParser>(),
            container2.Resolve<ILogger>()
        );

        processor2.ProcessTrades();
    }
}

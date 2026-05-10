// ╔══════════════════════════════════════════════════════════════════════╗
//  DI TECHNIQUE 3 — IoC Container  (used in real production apps)
//
//  How it works:
//    An IoC (Inversion of Control) container is a registry.
//    1. REGISTER  — tell the container: "when someone asks for ILogger,
//                   give them a ConsoleLogger."
//    2. RESOLVE   — ask the container for a type; it builds the whole
//                   dependency tree automatically.
//
//  Pros:
//    ✔ Zero manual wiring — container resolves nested dependencies for you.
//    ✔ Swap an implementation in ONE place (the registration).
//    ✔ Manages object lifetime (singleton, transient, scoped).
//    ✔ Industry-standard in ASP.NET Core, which has a built-in container.
//
//  Cons:
//    ✗ Magic — errors appear at runtime if a registration is missing.
//    ✗ Adds a learning curve for beginners.
//
//  The ServiceCollection here is the CUSTOM container built in this project.
//  In real ASP.NET Core apps, Microsoft.Extensions.DependencyInjection
//  does the same job with more features (lifetimes, decorators, etc.).
//
//  ─────────────────────────────────────────────────────────────────────
//  DIP vs DI — Final Summary
//  ─────────────────────────────────────────────────────────────────────
//  DIP  = the RULE:    "Depend on abstractions, not concretions."
//  DI   = the TOOL:    "Here is HOW you deliver those abstractions."
//  IoC  = the PATTERN: "The container, not you, controls who gets what."
//
//  You can have DIP without DI  (use a factory — less flexible).
//  You cannot meaningfully use DI without DIP (you'd inject concretions).
//  ─────────────────────────────────────────────────────────────────────
// ╚══════════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.DependencyInjection;
using TradingSolidPrinciples.Interfaces;
using TradingSolidPrinciples.TradeServices;
using TradingSolidPrinciples.TradingProcessing;
using TradingSolidPrinciples.TradingProcessing.Interfaces;

namespace TradingSolidPrinciples.DIP.DI_Techniques._3_IoCContainer;

public static class Program_IoCDemo
{
    public static void Run()
    {
        // ── STEP 1: REGISTER ─────────────────────────────────────────
        // Tell the container which concrete class satisfies each interface.
        // Nothing is instantiated yet — just a mapping is stored.

        var stream    = File.OpenRead(@"G:\Technical Data\AllData.csv");
        var container = new ServiceCollection();

        container
            // ILogger → ConsoleLogger
            // Want to switch to FileLogger? Change ONE line here. Nothing else changes.
            .Register<ILogger, ConsoleLogger>()

            // ITradeDataProvider → StreamTradeDataProvider (needs a Stream — use factory overload)
            .Register<ITradeDataProvider, StreamTradeDataProvider>(
                () => new StreamTradeDataProvider(stream))

            // ITradeValidator → SimpleTradeValidator (needs ILogger — resolve it from container)
            .Register<ITradeValidator, SimpleTradeValidator>(
                () => new SimpleTradeValidator(container.Resolve<ILogger>()))

            // ITradeMapper → SimpleTradeMapper
            .Register<ITradeMapper, SimpleTradeMapper>()

            // ITradeParser → SimpleTradeParser (needs ITradeValidator + ITradeMapper)
            .Register<ITradeParser, SimpleTradeParser>(
                () => new SimpleTradeParser(
                    container.Resolve<ITradeValidator>(),
                    container.Resolve<ITradeMapper>()))

            // ITradeStorage → AdoNetTradeStorage (needs ILogger)
            .Register<ITradeStorage, AdoNetTradeStorage>(
                () => new AdoNetTradeStorage(container.Resolve<ILogger>()))

            // ITradeProcessor → TradeProcessor (needs all three above)
            .Register<ITradeProcessor, TradeProcessor>(
                () => new TradeProcessor(
                    container.Resolve<ITradeDataProvider>(),
                    container.Resolve<ITradeStorage>(),
                    container.Resolve<ITradeParser>()));

        // ── STEP 2: RESOLVE ──────────────────────────────────────────
        // Ask the container for the top-level type.
        // It builds the ENTIRE dependency tree automatically.
        // The caller never calls new() — the container does it all.

        var processor = container.Resolve<ITradeProcessor>();  // ← whole tree resolved here
        processor.ProcessTrades();

        // ── What just happened under the hood ────────────────────────
        //  container resolved ITradeProcessor
        //    → created TradeProcessor
        //      → resolved ITradeDataProvider → created StreamTradeDataProvider(stream)
        //      → resolved ITradeStorage      → created AdoNetTradeStorage(consoleLogger)
        //      → resolved ITradeParser       → created SimpleTradeParser(validator, mapper)
        //  All of this from ONE line: container.Resolve<ITradeProcessor>()
    }
}

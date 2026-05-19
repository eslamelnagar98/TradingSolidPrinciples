namespace SD_PD_IDA_46.OCP;

// ╔══════════════════════════════════════════════════════════════════════╗
//  OCP — ConsoleLogger  (Extension 1 ✔)
//
//  Requirement: "Log everything to the console."
//
//  OCP applied:
//   ✔  ILogger (the base contract) is NEVER touched.
//   ✔  This new behaviour is delivered by creating THIS new class.
//   ✔  TradeProcessor and any other caller keep working unchanged.
//
//  Based on the static TradeLogger from SD_PD_IDA_46.SRP — but now
//  wrapped in a proper class that honours the ILogger contract, making
//  it injectable, testable, and swappable.
// ╚══════════════════════════════════════════════════════════════════════╝
public sealed class ConsoleLogger : ILogger
{
    public void LogInfo(string message, params object[] args) =>
        Console.WriteLine($"[INFO]  {DateTime.Now:HH:mm:ss} | {string.Format(message, args)}");

    public void LogWarning(string message, params object[] args) =>
        Console.WriteLine($"[WARN]  {DateTime.Now:HH:mm:ss} | {string.Format(message, args)}");

    public void LogException(string message, Exception exception) =>
        Console.WriteLine(
            $"[ERROR] {DateTime.Now:HH:mm:ss} | {message} " +
            $"— {exception.Message}{Environment.NewLine}{exception.StackTrace}");
}

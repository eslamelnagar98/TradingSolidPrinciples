namespace SD_PD_IDA_46.OCP;

// ╔══════════════════════════════════════════════════════════════════════╗
//  OCP — FileLogger  (Extension 2 ✔)
//
//  Requirement: "Log everything to a file with a timestamped name."
//
//  OCP applied:
//   ✔  ILogger (the base contract) is NEVER touched.
//   ✔  ConsoleLogger is NEVER touched.
//   ✔  This new behaviour is delivered by creating THIS new class.
//   ✔  TradeProcessor and any other caller keep working unchanged —
//      just swap ConsoleLogger for FileLogger at the composition root.
//
//  Difference from the static TradeLogger in SD_PD_IDA_46.SRP:
//    The static version could never be swapped, mocked, or tested.
//    This class implements ILogger so it is fully injectable.
// ╚══════════════════════════════════════════════════════════════════════╝
public sealed class FileLogger : ILogger
{
    private readonly string _filePath =
        Path.Combine(@"G:\Technical Data", $"{DateTime.Now:yyyyMMddHHmmss}.log");

    public FileLogger()
    {
        var directory = Path.GetDirectoryName(_filePath)!;
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (!File.Exists(_filePath))
            File.WriteAllText(_filePath, string.Empty);
    }

    public void LogInfo(string message, params object[] args) =>
        Append("INFO ", string.Format(message, args));

    public void LogWarning(string message, params object[] args) =>
        Append("WARN ", string.Format(message, args));

    public void LogException(string message, Exception exception) =>
        Append("ERROR",
            $"{message} — {exception.Message}{Environment.NewLine}{exception.StackTrace}");

    private void Append(string level, string content) =>
        File.AppendAllText(
            _filePath,
            $"[{level}] {DateTime.Now:yyyyMMddHHmmssffff} | {content}{Environment.NewLine}");
}

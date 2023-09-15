using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradeServices;
public class FileLogger : ILogger
{
    private readonly string _filePath = $@"G:\Technical Data\AllData_20230909173104.Log";
    public FileLogger()
    {
        CheckIfFileExits();
    }
    public void LogInformation(string message, params object[] helperData)
    {
        LogMessage("INFO", message, helperData);
    }
    public void LogWarning(string message, params object[] helperData)
    {
        LogMessage("WARN", message, helperData);
    }
    private void LogMessage(string notificationType, string message, params object[] args)
    {
        var content = $"{notificationType}| {DateTime.Now:yyyyMMddHHmmss}:Message:{message}, With Data: {string.Join(',', args)}.";
        File.AppendAllText(_filePath, $"{content}{Environment.NewLine}");
    }
    private void CheckIfFileExits()
    {
        if (!File.Exists(_filePath))
        {
            File.Create(_filePath, 1000, FileOptions.RandomAccess);
        }
    }
}

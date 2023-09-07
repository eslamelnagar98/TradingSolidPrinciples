using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradeServices;
public class FileLogger : ILogger
{
    private static readonly string _filePath = @$"G:\Technical Data\{DateTime.Now:yyyyMMddHHmmss}.Log";
    static FileLogger()
    {
        if (!File.Exists(_filePath))
        {
            File.Create(_filePath);
        }
    }
    public async Task LogException(string message, Exception exception)
    {
        var cotent = $"Error| {DateTime.Now:yyyyMMddHHmmssffff}:Message: {message}, " +
                     $"With Exception:{exception.Message}, StackTrace:{exception.StackTrace}";
        await File.AppendAllTextAsync(_filePath, cotent);
    }

    public async Task LogInformation(string message, params object[] heloperData)
    {
        var cotent = $"INFO| {DateTime.Now:yyyyMMddHHmmssffff}:Message: {message}, " +
                     $"With Data : {string.Join(',', heloperData)}";
        await File.AppendAllTextAsync(_filePath, cotent);
    }

    public async Task LogWarning(string message, params object[] heloperData)
    {
        var cotent = $"Warn| {DateTime.Now:yyyyMMddHHmmssffff}:Message: {message}, " +
                    $"With Data : {string.Join(',', heloperData)}";
        await File.AppendAllTextAsync(_filePath, cotent);
    }
}

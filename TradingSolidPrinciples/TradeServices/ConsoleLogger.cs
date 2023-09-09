using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradeServices;
public class ConsoleLogger : ILogger
{
    public void LogInformation(string message, params object[] helperData)
    {
        Console.WriteLine(message, helperData);
    }

    public void LogWarning(string message, params object[] helperData)
    {
        Console.WriteLine(message, helperData);
    }
}

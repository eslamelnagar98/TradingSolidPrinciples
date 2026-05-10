using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples;

public class FileLogger : ILogger
{
    public void LogInformation(string message, params object[] helperData)
    {
        //write to file 
    }

    public void LogWarning(string message, params object[] helperData)
    {
        //write to file 
    }
}

namespace TradingSolidPrinciples.Interfaces;
public interface ILogger
{
    void LogInformation(string message, params object[] helperData);
    void LogWarning(string message, params object[] helperData);
}

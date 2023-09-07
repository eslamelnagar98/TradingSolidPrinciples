namespace TradingSolidPrinciples.Interfaces;
public interface ILogger
{
    Task LogInformation(string message, params object[] heloperData);
    Task LogWarning(string message, params object[] heloperData);
    Task LogException(string message, Exception exception);
}

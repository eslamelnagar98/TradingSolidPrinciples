using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradeServices;
public class SimpleTradeValidator : ITradeValidator
{
    private readonly ILogger _logger;
    public SimpleTradeValidator(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public bool Validate(string[] tradeData)
    {
        if (tradeData.Length != 3)
        {
            _logger.LogWarning("Line malformed. Only field(s) found.", tradeData.Length);
            return false;
        }
        if (tradeData[0].Length != 6)
        {
            _logger.LogWarning("Trade currencies malformed", tradeData[0]);
            return false;
        }
        if (!int.TryParse(tradeData[1], out var _))
        {
            _logger.LogWarning("Trade amount not a valid integer", tradeData[1]);
            return false;
        }
        if (!decimal.TryParse(tradeData[2], out var _))
        {
            _logger.LogWarning("Trade price not a valid decimal: '{1}'",tradeData[2]);
            return false;
        }
        return true;
    }
}

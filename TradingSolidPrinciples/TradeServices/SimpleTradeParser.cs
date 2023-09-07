using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradeServices;
public class SimpleTradeParser : ITradeParser
{
    private readonly ITradeValidator _tradeValidator;
    private readonly ITradeMapper _tradeMapper;
    public SimpleTradeParser(ITradeValidator tradeValidator, ITradeMapper tradeMapper)
    {
        _tradeValidator = tradeValidator;
        _tradeMapper = tradeMapper;
    }
    public IEnumerable<Trade> Parse(IEnumerable<string> lines)
    {
        var lineCount = 1;
        foreach (var line in lines)
        {
            var fields = line.Split(',');
            if (!_tradeValidator.Validate(fields))
            {
                continue;
            }
            var trade = _tradeMapper.Map(fields);
            lineCount++;
            yield return trade;
        }
    }
}

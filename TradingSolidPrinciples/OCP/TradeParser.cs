using SD_PD_IDA_46.SRP;
using SD_PD_IDA_46.SRP.Models;

namespace SD_PD_IDA_46.OCP;

public sealed class TradeParser : ITradeParser
{
    private readonly TradeValidator _tradeValidator = new();
    private readonly TradeMapper _tradeMapper = new();

    public IEnumerable<Trade> Parse(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var fields = line.Split(',');
            if (!_tradeValidator.IsValid(fields))
                continue;
            yield return _tradeMapper.Map(fields);
        }
    }
}

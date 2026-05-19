using System.Diagnostics;
using SD_PD_IDA_46.SRP;

namespace SD_PD_IDA_46.OCP;

// CLOSED for modification: this class never needs to change when new
// data providers or storage mechanisms are introduced.
// OPEN for extension: behaviour is extended by injecting different
// implementations of ITradeDataProvider, ITradeParser, and ITradeStorage.
public sealed class TradeProcessor(
    ITradeDataProvider dataProvider,
    ITradeParser parser,
    ITradeStorage storage)
{
    public void ProcessTrades()
    {
        var sw = Stopwatch.StartNew();
        var rawData = dataProvider.GetTradeData();
        var trades = parser.Parse(rawData);
        storage.Persist(trades);
        sw.Stop();
        TradeLogger.LogInfo($"Trade processing completed in {sw.ElapsedMilliseconds} ms");
    }
}

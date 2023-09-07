using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradeServices;
public class StreamTradeDataProvider : ITradeDataProvider
{
    private readonly Stream _stream;
    public StreamTradeDataProvider(Stream stream)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }
    public IEnumerable<string> GetTradeData()
    {
        using var reader = new StreamReader(_stream);
        var line = default(string);
        while ((line = reader.ReadLine()) is not null)
        {
            yield return line;
        }
    }
}

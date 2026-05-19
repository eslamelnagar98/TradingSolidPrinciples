namespace SD_PD_IDA_46.OCP;

// Existing implementation — implements the interface, no changes needed to TradeProcessor
public sealed class StreamTradeDataProvider(Stream stream) : ITradeDataProvider
{
    public IEnumerable<string> GetTradeData()
    {
        using var reader = new StreamReader(stream);
        var line = string.Empty;
        while ((line = reader.ReadLine()) is not null)
        {
            yield return line;
        }
    }
}

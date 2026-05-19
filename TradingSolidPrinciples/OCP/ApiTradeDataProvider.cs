namespace SD_PD_IDA_46.OCP;

// NEW extension: fetches trade data from a remote API endpoint.
// TradeProcessor is never touched — we simply swap the provider.
public sealed class ApiTradeDataProvider(string apiUrl) : ITradeDataProvider
{
    public IEnumerable<string> GetTradeData()
    {
        // Simulates lines received from a remote API response
        Console.WriteLine($"[ApiTradeDataProvider] Fetching trades from {apiUrl} ...");
        yield return "EUR,USD,100,1.12";
        yield return "GBP,USD,200,1.27";
        yield return "USD,JPY,50,149.50";
    }
}

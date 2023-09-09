namespace TradingSolidPrinciples.Entities;
public sealed class Trade
{
    public string SourceCurrency { get; set; }
    public string DestinationCurrency { get; set; }
    public decimal Price { get; set; }
}

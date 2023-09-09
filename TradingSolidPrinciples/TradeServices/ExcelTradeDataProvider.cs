using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples.TradeServices;
public class ExcelTradeDataProvider : ITradeDataProvider
{
    private readonly string _excelFilePath;

    public ExcelTradeDataProvider(string excelFilePath)
    {
        _excelFilePath = excelFilePath;
    }
    public IEnumerable<string> GetTradeData()
    {
        return new List<string> { File.ReadAllText(_excelFilePath) };
    }
}

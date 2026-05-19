using SD_PD_IDA_46.SRP.Models;

namespace SD_PD_IDA_46.OCP;

public interface ITradeParser
{
    IEnumerable<Trade> Parse(IEnumerable<string> lines);
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace URLParser
{
    public interface IParserService
    {
        Task<List<ParsedLink>> Parse(IParserQuery query);
    }
}

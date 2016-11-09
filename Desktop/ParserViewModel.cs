using System.Collections.Generic;
using URLParser;

namespace Desktop
{
    public class ParserViewModel
    {
        public string Url { get; set; }
        public string Depth { get; set; }  
        public string Source { get; set; }
        public bool IsEnabled { get; private set; } = true;

        private readonly IParserService _parserService;

        public NotifyTaskCompletion<List<ParsedLink>> ParsedLinks
        {
            get
            {
                IsEnabled = false;
                var links =  new NotifyTaskCompletion<List<ParsedLink>>(_parserService.Parse(new ParserQuery
                {
                    Depth = int.Parse(Depth),
                    StartUrl = Url,
                    StartSource = Source
                }));
                IsEnabled = true;
                return links;
            }
        }

        public ParserViewModel(IParserService parserService)
        {
            _parserService = parserService;
        }
    }
}

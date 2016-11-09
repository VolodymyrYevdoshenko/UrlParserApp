namespace URLParser
{
    public class ParsedLink
    {
        public string Url { get; set; } = @"http://eleks.com/";
        public string SourceDirectory { get; set; } = @"C:\";

        public ParsedLink()
        {
            
        }
        public ParsedLink(string url, string source)
        {
            Url = url;
            SourceDirectory = source;
        }
    }
}

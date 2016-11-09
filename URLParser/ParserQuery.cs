namespace URLParser
{
    public class ParserQuery : IParserQuery
    {
        public int Depth { get; set; } = 10;
        public string StartUrl { get; set; } = @"http://eleks.com/";
        public string StartSource { get; set; } = @"C:\";
    }
}

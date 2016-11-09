namespace URLParser
{
    public interface IParserQuery
    {
        int Depth { get; set; }
        string StartUrl { get; set; }
        string StartSource { get; set; }
    }
}

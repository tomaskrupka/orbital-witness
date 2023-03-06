namespace LeasesApi.Parsers
{
    public record struct ParsedEntry(List<TextBlock> textBlocks, List<string> notes)
    {
        public static implicit operator (List<TextBlock> textBlocks, List<string> notes)(ParsedEntry value)
        {
            return (value.textBlocks, value.notes);
        }

        public static implicit operator ParsedEntry((List<TextBlock> textBlocks, List<string> notes) value)
        {
            return new ParsedEntry(value.textBlocks, value.notes);
        }
    }
}

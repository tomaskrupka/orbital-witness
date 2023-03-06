namespace LeasesApi.Parsers
{
    public interface IScheduleNoticeOfLeaseParser
    {
        public ParsedEntry Parse(List<string> entryText);
    }
}
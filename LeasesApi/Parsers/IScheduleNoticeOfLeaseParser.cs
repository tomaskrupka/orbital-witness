using LeasesApi.DTOs;

namespace LeasesApi.Parsers
{
    public interface IScheduleNoticeOfLeaseParser
    {
        public List<TextBlock> Parse(List<string> entryText);
    }
}
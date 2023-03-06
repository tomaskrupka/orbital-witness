using LeasesApi.DTOs;
using LeasesApi.Parsers;

namespace LeasesApi.Mappers
{
    public class ScheduleNoticeOfLeaseMapper : IScheduleNoticeOfLeaseMapper
    {
        private readonly IScheduleNoticeOfLeaseParser _scheduleNoticeOfLeaseParser;

        public ScheduleNoticeOfLeaseMapper(IScheduleNoticeOfLeaseParser scheduleNoticeOfLeaseParser)
        {
            _scheduleNoticeOfLeaseParser = scheduleNoticeOfLeaseParser;
        }
        public ParsedScheduleNoticeOfLease Map(RawScheduleNoticeOfLease rawScheduleNoticeOfLease)
        {
            var parsedNotice = new ParsedScheduleNoticeOfLease();

            parsedNotice.EntryNumber = int.Parse(rawScheduleNoticeOfLease.EntryNumber);

            parsedNotice.EntryDate =
                DateOnly.TryParse(rawScheduleNoticeOfLease.EntryDate, out var entryDate) ?
                entryDate :
                null;

            var (textBlocks, notes) = _scheduleNoticeOfLeaseParser.Parse(rawScheduleNoticeOfLease.EntryText);

            parsedNotice.RegistrationDateAndPlanRef = textBlocks[0].Text;
            parsedNotice.PropertyDescription = textBlocks[1].Text;
            parsedNotice.DateOfLeaseAndTerm = textBlocks[2].Text;
            parsedNotice.LesseesTitle = textBlocks[3].Text;

            parsedNotice.Notes = notes;

            return parsedNotice;
        }
    }
}

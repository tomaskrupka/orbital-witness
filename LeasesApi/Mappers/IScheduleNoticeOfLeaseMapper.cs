using LeasesApi.DTOs;

namespace LeasesApi.Mappers
{
    public interface IScheduleNoticeOfLeaseMapper
    {
        public ParsedScheduleNoticeOfLease Map(RawScheduleNoticeOfLease rawScheduleNoticeOfLease);
    }
}
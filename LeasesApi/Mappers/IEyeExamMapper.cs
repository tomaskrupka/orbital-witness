using LeasesApi.DTOs;

namespace LeasesApi.Mappers
{
    public interface IEyeExamMapper
    {
        Task<ParsedScheduleNoticeOfLease> MapScheduleNoticeOfLeaseAsync(RawScheduleNoticeOfLease rawScheduleNoticeOfLease);
    }
}
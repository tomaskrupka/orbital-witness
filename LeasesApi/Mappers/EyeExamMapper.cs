using LeasesApi.DTOs;

namespace LeasesApi.Mappers
{
    public class EyeExamMapper : IEyeExamMapper
    {
        private readonly IScheduleNoticeOfLeaseMapper _scheduleNoticeOfLeaseMapper;

        public EyeExamMapper(IScheduleNoticeOfLeaseMapper scheduleNoticeOfLeaseMapper)
        {
            _scheduleNoticeOfLeaseMapper = scheduleNoticeOfLeaseMapper;
        }
        public Task<ParsedScheduleNoticeOfLease> MapScheduleNoticeOfLeaseAsync(RawScheduleNoticeOfLease rawScheduleNoticeOfLease)
        {
            return Task.Run(() => _scheduleNoticeOfLeaseMapper.Map(rawScheduleNoticeOfLease));
        }
    }
}

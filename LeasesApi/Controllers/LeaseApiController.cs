using LeasesApi.EyeExam;
using LeasesApi.DTOs;
using LeasesApi.Mappers;
using LeasesApi.Serialisers;
using Microsoft.AspNetCore.Mvc;

namespace LeasesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeaseApiController : ControllerBase
    {
        private readonly IEyeExamClient _eyeExamClient;
        private readonly IEyeExamSerialiser _eyeExamSerialiser;
        private readonly IEyeExamMapper _eyeExamMapper;
        private readonly IScheduleNoticeOfLeaseMapper _scheduleNoticeOfLeaseMapper;

        public LeaseApiController(
            IEyeExamClient eyeExamClient,
            IEyeExamSerialiser eyeExamApiSerialiser,
            IEyeExamMapper eyeExamMapper,
            IScheduleNoticeOfLeaseMapper scheduleNoticeOfLeaseMapper)
        {
            _eyeExamClient = eyeExamClient;
            _eyeExamSerialiser = eyeExamApiSerialiser;
            _eyeExamMapper = eyeExamMapper;
            _scheduleNoticeOfLeaseMapper = scheduleNoticeOfLeaseMapper;
        }

        [HttpGet]
        [Route("ScheduleNotices")]
        public async Task<IEnumerable<ParsedScheduleNoticeOfLease>> GetScheduleNoticesOfLeaseAsync()
        {
            // TODO: This is a blocking routine. See if the worst case performance is acceptable and if not, respond with a promise and process asynchronously.
            var stream = await _eyeExamClient.GetRawScheduleNoticeOfLeasesAsync();
            var dto = await _eyeExamSerialiser.DeserialiseAsync<List<RawScheduleNoticeOfLease>>(stream);
            var parsed = await Task.WhenAll(dto.Select(x => _eyeExamMapper.MapScheduleNoticeOfLeaseAsync(x))).ContinueWith(x => x.Result);
            return parsed;
        }

        [HttpGet]
        [Route("ScheduleNotices/{Id:int}")]
        public ParsedScheduleNoticeOfLease GetScheduleNoticeOfLease()
        {
            return new ParsedScheduleNoticeOfLease();
        }
    }
}
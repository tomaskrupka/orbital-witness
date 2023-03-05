using LeasesApi.EyeExam.Extensions;
using Microsoft.Extensions.Options;

namespace LeasesApi.EyeExam
{
    public class EyeExamClient : IEyeExamClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<EyeExamClientOptions> _optionsMonitor;

        public EyeExamClient(IHttpClientFactory httpClientFactory, IOptionsMonitor<EyeExamClientOptions> optionsMonitor)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(EyeExamClient));
            _optionsMonitor = optionsMonitor;

            _optionsMonitor.OnChange(x => _httpClient.UpdateSettings(x));
        }

        public Task<Stream> GetRawScheduleNoticeOfLeasesAsync()
        {
            return _httpClient.GetStreamAsync("schedules");
        }
    }
}

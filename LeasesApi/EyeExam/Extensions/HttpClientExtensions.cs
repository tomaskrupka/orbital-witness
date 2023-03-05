namespace LeasesApi.EyeExam.Extensions
{
    public static class HttpClientExtensions
    {
        public static void UpdateSettings(this HttpClient httpClient, EyeExamClientOptions options)
        {
            httpClient.BaseAddress = new Uri(options.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(options.ApiUsername, options.ApiPassword);
        }
    }
}

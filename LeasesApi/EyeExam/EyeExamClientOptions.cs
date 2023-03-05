namespace LeasesApi.EyeExam
{
    public class EyeExamClientOptions
    {
        public const string Key = "EyeExamClientOptions";
        public string ApiUsername { get; set; } = string.Empty;
        public string ApiPassword { get; set; } = string.Empty;
        public string BaseAddress { get; set; } = string.Empty;
    }
}

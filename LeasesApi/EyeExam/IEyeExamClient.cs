namespace LeasesApi.EyeExam
{
    public interface IEyeExamClient
    {
        Task<Stream> GetRawScheduleNoticeOfLeasesAsync();
    }
}
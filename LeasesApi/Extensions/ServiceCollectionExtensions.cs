using LeasesApi.EyeExam;
using LeasesApi.EyeExam.Extensions;
using LeasesApi.Mappers;
using LeasesApi.Parsers;
using LeasesApi.Serialisers;

namespace LeasesApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLeasesApi(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddEyeExamHttpClient(configuration)
                .AddSingleton<IEyeExamClient, EyeExamClient>()
                .AddSingleton<IEyeExamMapper, EyeExamMapper>()
                .AddSingleton<IScheduleNoticeOfLeaseMapper, ScheduleNoticeOfLeaseMapper>()
                .AddSingleton<IScheduleNoticeOfLeaseParser, ScheduleNoticeOfLeaseParser>()
                .AddSingleton<IEyeExamSerialiser, EyeExamSerialiser>();

            return services;
        }
    }
}

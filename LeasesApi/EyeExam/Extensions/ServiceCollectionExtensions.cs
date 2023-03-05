using Microsoft.Extensions.Options;
using System.Net.Http;

namespace LeasesApi.EyeExam.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add and configure an HttpClient for the EyeExamApi integration.
        /// </summary>
        /// <param name="services">The ServiceCollection to inject everything into.</param>
        /// <param name="configuration">Must contain a <see cref="nameof(EyeExamClient)"/> section.</param>
        /// <returns>The same collection, useful for chaining.</returns>
        /// <remarks>
        /// New HttpClients will come out of the factory preconfigured with what was in the config file at the time this extension was executed, which may already be out of date.
        /// The owner of the HttpClient instance can keep its configuration up-to-date by subscribing to an <see cref="IOptionsMonitor{TOptions}"/> and/or
        /// calling <see cref="HttpClientExtensions.UpdateSettings(HttpClient, EyeExamClientOptions)"/> with the current configuration value manually.
        /// </remarks>
        public static IServiceCollection AddEyeExamHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var eyeExamConfigSection = configuration.GetSection(EyeExamClientOptions.Key);
            var initialOptions = new EyeExamClientOptions();

            eyeExamConfigSection.Bind(initialOptions);

            services
                .AddHttpClient(nameof(EyeExamClient))
                .ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.All
                })
                .ConfigureHttpClient((_, httpClient) =>
                {
                    httpClient.UpdateSettings(initialOptions);
                });

            services.Configure<EyeExamClientOptions>(eyeExamConfigSection);

            return services;
        }
    }
}

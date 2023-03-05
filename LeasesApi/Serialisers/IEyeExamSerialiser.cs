using System.Text.Json;

namespace LeasesApi.Serialisers
{
    public interface IEyeExamSerialiser
    {
        JsonSerializerOptions DefaultJsonSerializerOptions { get; set; }

        Task<T> DeserialiseAsync<T>(Stream stream);
        Task<T> DeserialiseAsync<T>(Stream stream, JsonSerializerOptions jsonSerializerOptions);
    }
}
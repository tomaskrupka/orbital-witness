using System.Text.Json;

namespace LeasesApi.Serialisers
{
    public class EyeExamSerialiser : IEyeExamSerialiser
    {
        public JsonSerializerOptions DefaultJsonSerializerOptions { get; set; }

        public EyeExamSerialiser()
        {
            // TODO: Handle common integration API quirks by providing custom serializer options. Either hardcoded here or injected as IConfiguration
            DefaultJsonSerializerOptions = new(JsonSerializerDefaults.Web);
        }

        public Task<T> DeserialiseAsync<T>(Stream stream)
        {
            return DeserialiseAsync<T>(stream, DefaultJsonSerializerOptions);
        }

        public Task<T> DeserialiseAsync<T>(Stream stream, JsonSerializerOptions jsonSerializerOptions)
        {
            return JsonSerializer.DeserializeAsync<T>(stream, jsonSerializerOptions).AsTask().ContinueWith(x => ValidateDeserialisation(x.Result));
        }

        private static T ValidateDeserialisation<T>(T? deserialisationResult)
        {
            if (deserialisationResult is null)
            {
                throw new SerialiserException(); // TODO: Provide some info. What was in the stream? What were the options?
            }

            return deserialisationResult;
        }
    }
}

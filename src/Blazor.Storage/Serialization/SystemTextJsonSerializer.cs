using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Blazored.Storage.Serialization
{
    internal class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonSerializer(IOptions<StorageOptions> options)
        {
            _options = options.Value.JsonSerializerOptions;
        }

        public SystemTextJsonSerializer(StorageOptions StorageOptions)
        {
            _options = StorageOptions.JsonSerializerOptions;
        }

        public T? Deserialize<T>(string data) 
            => JsonSerializer.Deserialize<T>(data, _options);

        public string Serialize<T>(T data)
            => JsonSerializer.Serialize(data, _options);
    }
}

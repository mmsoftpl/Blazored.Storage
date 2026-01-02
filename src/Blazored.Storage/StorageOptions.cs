using System.Text.Json;

namespace Blazored.Storage;

public class StorageOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new();
}

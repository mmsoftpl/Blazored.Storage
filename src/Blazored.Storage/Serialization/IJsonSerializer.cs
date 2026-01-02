namespace Blazored.Storage.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);
        T? Deserialize<T>(string text);
    }
}

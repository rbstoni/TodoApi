namespace TodoApi.Application.Common.Interfaces
{
    public interface ISerializerService : ITransientService
    {

        T? Deserialize<T>(string text);
        string Serialize<T>(T obj);
        string Serialize<T>(T obj, Type type);

    }
}
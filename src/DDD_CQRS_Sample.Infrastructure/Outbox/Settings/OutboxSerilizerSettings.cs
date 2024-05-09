using Newtonsoft.Json;

namespace DDD_CQRS_Sample.Infrastructure.Outbox.Settings
{
    public class OutboxSerilizerSettings
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };
    }
}

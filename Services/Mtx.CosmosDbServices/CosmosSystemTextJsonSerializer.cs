namespace Mtx.CosmosDbServices
{
    using Azure.Core.Serialization;
    using Microsoft.Azure.Cosmos;
    using System.IO;
    using System.Text.Json;

    /// <summary>
    /// Uses <see cref="Azure.Core.Serialization.JsonObjectSerializer"/> which leverages System.Text.Json providing a simple API to interact with on the Azure SDKs.
    /// </summary>
    // <SystemTextJsonSerializer>
#pragma warning disable CS8603 
#pragma warning disable CS8600 
    public class CosmosSystemTextJsonSerializer : CosmosSerializer
    {
        private readonly JsonObjectSerializer systemTextJsonSerializer;

        public CosmosSystemTextJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            this.systemTextJsonSerializer = new JsonObjectSerializer(jsonSerializerOptions);
        }

        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (stream.CanSeek
                       && stream.Length == 0)
                {
                    return default;
                }

                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)stream;
                }

                return (T)systemTextJsonSerializer?.Deserialize(stream, typeof(T), default);
            }
        }

        public override Stream ToStream<T>(T input)
        {
            MemoryStream streamPayload = new MemoryStream();
            this.systemTextJsonSerializer.Serialize(streamPayload, input, typeof(T), default);
            streamPayload.Position = 0;
            return streamPayload;
        }
    }
#pragma warning restore CS8603
#pragma warning restore CS8600
}

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mtx.CosmosDbServices
{
    public static class CosmosDbConfigOptionsExtensions
    {
        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key. 
        /// </summary>
        /// <returns></returns>
        public static  void InitializeCosmosClientInstance( this IServiceCollection services,IConfiguration configuration)
        {
            string databaseName = configuration.GetSection("CosmosConfig:DbName").Value;
            string containerName = "Skills";
            string account = configuration.GetSection("CosmosConfig:Endpoint").Value;
            string key = configuration.GetSection("CosmosConfig:Key").Value;

            var options = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            };

            var clientOptions = new CosmosClientOptions
            {
                //SerializerOptions = options,
                Serializer = new CosmosJsonDotNetSerializer(new JsonSerializerSettings
                {

                    ContractResolver = new JsonDotNetPrivateResolver(),
                    TypeNameHandling = TypeNameHandling.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Error,
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                })

            };

            var client = new CosmosClient(account, key, clientOptions: clientOptions) ;
            var cosmosDbService = new CosmosDbService(client, databaseName, containerName);
            var database = client.CreateDatabaseIfNotExistsAsync(databaseName).Result;
            _= database.Database.CreateContainerIfNotExistsAsync(containerName, "/id").Result;
            services.AddSingleton<ICosmosDbService>(cosmosDbService);
        }
    }

}

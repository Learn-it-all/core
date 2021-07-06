using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Cosmos;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Infrastructure.EFCore;
using System;

namespace Mtx.LearnItAll.Core.API.Serverless.Azure.Infrastructure.Data
{
    public class CosmosDbContext : CoreDbContext
    {

        CosmosConfig _config;
        public CosmosDbContext(DbContextOptions<CosmosDbContext> options,IOptions<CosmosConfig> cosmosOptions) : base(options)
        {
            _config = cosmosOptions?.Value ?? throw new ArgumentNullException(nameof(cosmosOptions)); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseCosmos(accountEndpoint: _config.Endpoint,accountKey: _config.Key, databaseName: _config.DbName,
        options =>
        {
            options.ConnectionMode(ConnectionMode.Direct);
            //options.WebProxy(new WebProxy());
            //options.LimitToEndpoint();
            //options.Region(Regions.GermanyWestCentral);
            //options.GatewayModeMaxConnectionLimit(32);
            //options.MaxRequestsPerTcpConnection(8);
            //options.MaxTcpConnectionsPerEndpoint(16);
            //options.IdleTcpConnectionTimeout(TimeSpan.FromMinutes(1));
            //options.OpenTcpConnectionTimeout(TimeSpan.FromMinutes(1));
            //options.RequestTimeout(TimeSpan.FromMinutes(1));
        });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultContainer("Core");

            modelBuilder.Entity<SkillBlueprint>()
                .ToContainer("TopLevelSkills")
                //.HasPartitionKey(e => e.Id)
                .HasNoDiscriminator();//used when no other type is ever to be stored in the same container
        }
    }
}

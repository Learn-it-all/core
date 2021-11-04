using Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;

await Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        builder.AddEnvironmentVariables();
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            builder.AddUserSecrets<Program>();
        }
    })

#if DEBUG
        .UseEnvironment("Development") 
#endif
    .UseOrleans((host, silo) =>
    {
        silo
            .UseLocalhostClustering()
            .ConfigureApplicationParts(parts => parts
            .AddApplicationPart(typeof(ISkillBlueprintGrain).Assembly)
            .AddApplicationPart(typeof(SkillBlueprintGrain).Assembly)
            )
           .UseCosmosDBMembership(x =>
           {
               x.AccountEndpoint = host.Configuration["CosmosDBClusteringOptions:AccountEndpoint"];
               x.AccountKey = host.Configuration["CosmosDBClusteringOptions:AccountKey"];
               x.CanCreateResources = true;

           })
           .AddCosmosDBGrainStorageAsDefault(x =>
           {
               x.AccountKey = host.Configuration["CosmosDBStorageOptions:AccountKey"];
               x.AccountEndpoint = host.Configuration["CosmosDBStorageOptions:AccountEndpoint"];

           }).AddCosmosDBGrainStorage("skillBlueprintState",x =>
           {
               x.AccountKey = host.Configuration["CosmosDBStorageOptions:AccountKey"];
               x.AccountEndpoint = host.Configuration["CosmosDBStorageOptions:AccountEndpoint"];
           })
           ;
           //.AddCosmosDBGrainStorage(skillBlueprintState)
            //.UseCosmosDBReminderService(op =>
            //{
            //    op.DB = "";
            //    // Configure CosmosDB reminders provider.
            //    // Configure CosmosDB settings in opt.
            //})
    })
    .RunConsoleAsync();
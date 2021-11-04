using Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mtx.LearnItAll.Core.Common;
using Mtx.LearnItAll.Core.Common.Parts;
using Orleans;
using Orleans.Clustering.CosmosDB;
using Orleans.Hosting;
using Spectre.Console;
using System;
using System.Threading.Tasks;

var client = new ClientBuilder()
    .ConfigureAppConfiguration((host, builder) =>
    {
        builder.AddEnvironmentVariables();
        if ("Development".Equals(host.HostingEnvironment.EnvironmentName))
        {
            builder.AddUserSecrets<Program>();
        }
    })
    .UseLocalhostClustering()
    .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ISkillBlueprintGrain).Assembly).WithReferences())
    .UseCosmosDBGatewayListProvider(x =>
    {

        x.AccountEndpoint = "";
        x.AccountKey = "";


    })
    .UseEnvironment("Development")
    .Build();

await AnsiConsole.Status().StartAsync("Connecting to server", async ctx =>
{
    ctx.Spinner(Spinner.Known.Dots);
    ctx.Status = "Connecting...";

    await client.Connect(async error =>
    {
        AnsiConsole.MarkupLine("[bold red]Error:[/] error connecting to server!");
        AnsiConsole.WriteException(error);
        ctx.Status = "Waiting to retry...";
        await Task.Delay(TimeSpan.FromSeconds(2));
        ctx.Status = "Retrying connection...";
        return true;
    });

    ctx.Status = "Connected!";
});

try
{
    var userName = AnsiConsole.Ask<string>("What is the [aqua]name[/] of the Skill?");
    var grain = client.GetGrain<ISkillBlueprintGrain>(userName);
    await grain.Add(new Mtx.LearnItAll.Core.Blueprints.PartNode(new Name("if")));
    //grain.AddPart(newPart: new AddPartCmd(new Name("string"), grain.SkillId));
}
catch (Exception e)
{

    AnsiConsole.WriteException(e);
    Console.Read();
}
finally
{
    await client.Close();

}

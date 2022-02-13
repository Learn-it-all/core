using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Common.Parts;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.Handlers;

public class DeleteSkillBlueprintHandler : IRequestHandler<DeleteBlueprintCmd, DeleteBlueprintResult>
{
    private readonly ICosmosDbService cosmosDb;

    public DeleteSkillBlueprintHandler(ICosmosDbService cosmosDb)
    {
        this.cosmosDb = cosmosDb;
    }

    public async Task<DeleteBlueprintResult> Handle(DeleteBlueprintCmd request, CancellationToken cancellationToken)
    {
        try
        {
            await cosmosDb.DeleteAsync(request.BlueprintId.ToString());
            return await Task.FromResult(DeleteBlueprintResult.CreateSuccess);
        }
        catch
        {
            return await Task.FromResult(DeleteBlueprintResult.CreateInternalError);

        }
    }

}

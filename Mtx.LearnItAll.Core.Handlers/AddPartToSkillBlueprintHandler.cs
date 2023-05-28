using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Common.Parts;
using System.Threading;
using System.Threading.Tasks;
using Mtx.LearnItAll.Core.Blueprints;

namespace Mtx.LearnItAll.Core.Handlers
{
    public class AddPartToSkillBlueprintHandler : IRequestHandler<AddPartCmd, AddPartResult>, IRequestHandler<AddMultiplePartsCmd, AddMultiplePartsResult>
    {
        private readonly ICosmosDbService cosmosDb;

        public AddPartToSkillBlueprintHandler(ICosmosDbService cosmosDb)
        {
            this.cosmosDb = cosmosDb;
        }

        public async Task<AddPartResult> Handle(AddPartCmd request, CancellationToken cancellationToken)
        {
            var result = await cosmosDb.GetAsync<SkillBlueprint>(id: request.BlueprintId,partitionKey: request.BlueprintId, cancellationToken);
            if (result.IsSuccessAndHasValue)
            {
                var skill = result.Contents;
                if (result.Contents.TryAdd(request, out AddPartResult tryResult))
                    await cosmosDb.UpdateAsync(skill, skill.Id, skill.Id, cancellationToken) ;
                 return tryResult;
            }

            return result;
        }

        public async Task<AddMultiplePartsResult> Handle(AddMultiplePartsCmd request, CancellationToken cancellationToken)
        {
            var skill = await cosmosDb.GetAsync<SkillBlueprint>(request.BlueprintId);
            var result = skill.Add(request);
            if (!result.HasErrors)
                await cosmosDb.UpdateAsync(skill, skill.Id, skill.Id, cancellationToken);

            return result;
        }
    }
}

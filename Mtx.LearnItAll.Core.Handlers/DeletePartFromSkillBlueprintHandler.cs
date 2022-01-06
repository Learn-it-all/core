using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Common.Parts;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.Handlers
{
    public class DeletePartFromSkillBlueprintHandler : IRequestHandler<DeletePartCmd, DeletePartResult>
    {
        private readonly ICosmosDbService cosmosDb;

        public DeletePartFromSkillBlueprintHandler(ICosmosDbService cosmosDb)
        {
            this.cosmosDb = cosmosDb;
        }

        public async Task<DeletePartResult> Handle(DeletePartCmd request, CancellationToken cancellationToken)
        {
            var skill = await cosmosDb.GetSkillBlueprintAsync(request.BlueprintId.ToString());
            if (skill.TryDeletePart(request, out DeletePartResult result))
                await cosmosDb.UpdateAsync(skill.Id.ToString(), skill);

            return result;
        }

    }
}

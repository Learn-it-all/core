using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.Handlers
{
    public class AddPartToSkillBlueprintHandler : IRequestHandler<AddPartCmd, AddPartResult>
    {
        private readonly ICosmosDbService cosmosDb;

        public AddPartToSkillBlueprintHandler(ICosmosDbService cosmosDb)
        {
            this.cosmosDb = cosmosDb;
        }

        public async Task<AddPartResult> Handle(AddPartCmd request, CancellationToken cancellationToken)
        {
            var skill = await cosmosDb.GetSkillBlueprintAsync(request.BlueprintId.ToString());
            if (skill.TryAdd(request, out AddPartResult result))
                await cosmosDb.UpdateAsync(skill.Id.ToString(), skill);

            return result;            
        }

    }
}

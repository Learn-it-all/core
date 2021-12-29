using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.Handlers
{
    public class AddPartToSkillBlueprintHandler : IRequestHandler<AddPartCmd,Guid>
    {
        private readonly ICosmosDbService cosmosDb;

        public AddPartToSkillBlueprintHandler(ICosmosDbService cosmosDb)
        {
            this.cosmosDb = cosmosDb;
        }

        public async Task<Guid> Handle(AddPartCmd request, CancellationToken cancellationToken)
        {
            var skill = await cosmosDb.GetSkillBlueprintAsync(request.BlueprintId.ToString());
            if (skill.TryAdd(request, out Guid partId))
            {
                await cosmosDb.UpdateAsync(skill.Id.ToString(), skill);
                return partId;
            }
            else
            {
                return Guid.Empty;
            }
            
        }

    }
}

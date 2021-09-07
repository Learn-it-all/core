using MediatR;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.Handlers
{
    public class CreateSkillBlueprintHandler : IRequestHandler<CreateSkillBlueprintCmd, Guid>
    {
        private readonly ICosmosDbService cosmosDb;

        public CreateSkillBlueprintHandler(ICosmosDbService cosmosDb)
        {
            this.cosmosDb = cosmosDb;
        }

        public async Task<Guid> Handle(CreateSkillBlueprintCmd request, CancellationToken cancellationToken)
        {
            var newBlueprint = new SkillBlueprint(request.Name);
            await cosmosDb.AddAsync(newBlueprint, cancellationToken);

            return newBlueprint.Id;
        }

    }
}

using MediatR;
using Mtx.LearnItAll.Core.Common.Parts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.LearnItAll.Core.Handlers
{
    public class CreateSkillBlueprintHandler : AsyncRequestHandler<CreateSkillBlueprintCmd>
    {

        protected override async Task Handle(CreateSkillBlueprintCmd request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;
using Mtx.LearnItAll.Core.Blueprints.SharedKernel;

namespace Mtx.LearnItAll.Core.Infrastructure.IoC
{
	public static class CoreDomainStartupExtensions
	{
		public static void InitializeCoreDomain(this IServiceCollection services, IConfiguration configuration)
		{
			ContainerFactory.IdentityMap.AddFor<PersonalSkill>(ContainerName.From("Skills"));
			ContainerFactory.IdentityMap.AddFor<SkillBluePrint>(ContainerName.From("Skills"));	
			ContainerFactory.IdentityMap.AddFor<SelectAllSkillBluePrintsNames>(ContainerName.From("skill-names-in-use"));	
			ContainerFactory.IdentityMap.AddFor<IsSkillBluePrintNameInUse>(ContainerName.From("skill-names-in-use"));	
		}
	}
}
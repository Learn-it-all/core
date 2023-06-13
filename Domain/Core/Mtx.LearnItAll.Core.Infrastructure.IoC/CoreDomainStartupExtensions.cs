using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Blueprints;

namespace Mtx.LearnItAll.Core.Infrastructure.IoC
{
	public static class CoreDomainStartupExtensions
	{
		public static void InitializeCoreDomain(this IServiceCollection services, IConfiguration configuration)
		{
			ContainerFactory.IdentityMap.AddFor<SkillBlueprint>(ContainerName.From("Skills"));
		}
	}
}
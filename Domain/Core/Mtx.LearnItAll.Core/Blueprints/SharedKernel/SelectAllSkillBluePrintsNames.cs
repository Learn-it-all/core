using Mtx.CosmosDbServices;
using Mtx.LearnItAll.Core.Common;

namespace Mtx.LearnItAll.Core.Blueprints.SharedKernel
{
	public record SelectAllSkillBluePrintsNames : CosmosQuery
	{
		private static string query = @$"SELECT * FROM c WHERE c.id = '{nameof(SkillBluePrint)}'";
		public SelectAllSkillBluePrintsNames() : base(query)
		{
		}

		public static SelectAllSkillBluePrintsNames Instance => new();
	}
}
using Mtx.CosmosDbServices;

namespace Mtx.LearnItAll.Core.Blueprints.SharedKernel
{
	public record IsSkillBluePrintNameInUse : CosmosQuery
	{
		public const string ParamNameForName = "@name";
		public static string query = @$"SELECT COUNT(1) AS total FROM c WHERE c.id = '{nameof(SkillBluePrint)}' AND c.name = {ParamNameForName}";
		
		public IsSkillBluePrintNameInUse(LowerCaseText name) : base(query)
		{
			Add(Param.Create(name, ParamNameForName));
		}

	}

}
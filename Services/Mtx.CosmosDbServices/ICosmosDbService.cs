using Mtx.LearnItAll.Core.Blueprints;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mtx.CosmosDbServices
{
    public interface ICosmosDbService
    {
        Task AddAsync(SkillBlueprint SkillBlueprint);
        Task DeleteAsync(string id);
        Task<IEnumerable<SkillBlueprint>> GetItemAsync(string queryString);
        Task<SkillBlueprint> GetSkillBlueprintAsync(string id);
        Task UpdateAsync(string id, SkillBlueprint skillBlueprint);
    }
}
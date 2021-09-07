using Mtx.LearnItAll.Core.Blueprints;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mtx.CosmosDbServices
{
    public interface ICosmosDbService
    {
        Task AddAsync(SkillBlueprint skillBlueprint, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<SkillBlueprint>> GetItemAsync(string queryString);
        Task<SkillBlueprint> GetSkillBlueprintAsync(string id);
        Task UpdateAsync(string id, SkillBlueprint skillBlueprint);
    }
}
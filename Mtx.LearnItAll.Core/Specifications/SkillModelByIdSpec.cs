using Ardalis.Specification;
using System;
using System.Linq;

namespace Mtx.LearnItAll.Core.Specifications
{
    public class SkillModelByIdSpec : Specification<SkillModel>, ISingleResultSpecification
    {
        public SkillModelByIdSpec(Guid id)
        {
            Query.Where(x => x.Id == id);

        }
    }
}

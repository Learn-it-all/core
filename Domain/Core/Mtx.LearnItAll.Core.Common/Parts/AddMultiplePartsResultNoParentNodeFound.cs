using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddMultiplePartsResultNoParentNodeFound : AddMultiplePartsResult
    {
        public AddMultiplePartsResultNoParentNodeFound(IEnumerable<Name> partNames)
        {
            foreach (var name in partNames)
                _results[name] = AddPartResult.FailureForPartNotFound;
        }

        public override bool HasErrors => true;
        public override bool Succeeded(Name name) => false;
    }
}

using Mtx.Common.Resources;
using Mtx.LearnItAll.Core.Resources;
using Mtx.Results;
using System;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddMultiplePartsResultNoParentNodeFound : AddMultiplePartsResult
    {
        public AddMultiplePartsResultNoParentNodeFound(IEnumerable<Name> partNames) : base(StatusCodes.Status404NotFound, CoreMessages.Skill_ParentIdDoesNotExist)
        {
            foreach (var name in partNames)
                _results[name] = AddPartResult.FailureForParentNodeNotFound;
        }

		public AddMultiplePartsResultNoParentNodeFound(int StatusCode, string Message = "", Exception Exception = null) : base(StatusCode, Message, Exception)
		{
		}

		public override bool HasErrors => true;
        public override bool Succeeded(Name name) => false;
    }
}

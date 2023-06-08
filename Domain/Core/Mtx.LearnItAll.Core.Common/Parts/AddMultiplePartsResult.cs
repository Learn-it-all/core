using Mtx.Results;
using System.Collections.Generic;

namespace Mtx.LearnItAll.Core.Common.Parts
{
    public record AddMultiplePartsResult
    {
        protected readonly Dictionary<Name, AddPartResult> _results = new();

        public Result Result { get; protected set; }
        public IReadOnlyDictionary<Name, AddPartResult> Results => _results;
        public virtual bool HasErrors { get; private set; }

        public void Add(Name name, AddPartResult result)
        {
            _results[name] = result;
            if (!result.IsSuccess) HasErrors = true;

        }

        public virtual bool Succeeded(Name name)
        {
            if (!_results.ContainsKey(name)) return false;
            return _results[name].IsSuccess;
        }


        public virtual IEnumerable<(Name Name, string Error)> Errors
        {
            get
            {
                foreach (var item in _results)
                {
                    if (!item.Value.IsSuccess) yield return new(item.Key, item.Value.Message);
                }
            }
        }
    }
}

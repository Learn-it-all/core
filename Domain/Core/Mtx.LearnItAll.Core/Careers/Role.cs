using Mtx.LearnItAll.Core.Common;

namespace Mtx.LearnItAll.Core
{
    public record Role
    {
        public Role(Name name)
        {
            this.Name = name;
        }

        public Name Name { get; }
    }
}
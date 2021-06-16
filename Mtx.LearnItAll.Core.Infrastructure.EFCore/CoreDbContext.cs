using Microsoft.EntityFrameworkCore;

namespace Mtx.LearnItAll.Core.Infrastructure.EFCore
{
    public class CoreDbContext : DbContext
    {
#pragma warning disable CS8618 
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
#pragma warning restore CS8618 
        : base(options)
        {

        }

        public DbSet<TopLevelSkill> TopLevelSkills { get; set; }
    }
}

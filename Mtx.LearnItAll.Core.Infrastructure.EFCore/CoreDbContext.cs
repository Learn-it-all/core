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

#pragma warning disable CS8618 
        public CoreDbContext(DbContextOptions options)
#pragma warning restore CS8618
        : base(options)
        {

        }

        public DbSet<TopLevelSkill> TopLevelSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TopLevelSkill>()
                .OwnsOne<SkillModel>("_root")
                .OwnsMany<SkillModel>("_skills");

            modelBuilder.Entity<TopLevelSkill>()
               .Ignore(x => x.Skills)
                .Property(x => x.ModifiedDate).ValueGeneratedOnUpdate();

            modelBuilder.Entity<TopLevelSkill>()
                .Property(x => x.CreatedDate).ValueGeneratedOnAdd();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Mtx.Common.Domain;
using System;
using System.Linq;

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

        public DbSet<Skill> TopLevelSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Skill>()
                .OwnsOne<SkillPart>("_root")
                .OwnsMany<SkillPart>("_skills");

            modelBuilder.Entity<Skill>()
               .Ignore(x => x.Skills)
               .Ignore(x => x.DomainEvents);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Entity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((Entity)entityEntry.Entity).ModifiedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Entity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}

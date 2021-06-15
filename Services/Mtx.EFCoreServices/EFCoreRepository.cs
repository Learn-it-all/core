using Microsoft.EntityFrameworkCore;
using Mtx.Common.Domain;
using Mtx.Common.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Mtx.EFCoreServices
{
    public class EFCoreRepository<TContext> : EFCoreReadOnlyRepository<TContext>, IRepository
      where TContext : DbContext
    {
        public EFCoreRepository(TContext context)
            : base(context)
        {
        }

        public virtual void Create<TEntity>(TEntity entity, string? createdBy = null)
            where TEntity : class, IEntity
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity, string? modifiedBy = null)
            where TEntity : class, IEntity
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = modifiedBy;
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete<TEntity>(object id)
            where TEntity : class, IEntity
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Save()
        {
                context.SaveChanges();
        }

        public virtual Task SaveAsync()
        {
                return context.SaveChangesAsync();
        }

    }
}

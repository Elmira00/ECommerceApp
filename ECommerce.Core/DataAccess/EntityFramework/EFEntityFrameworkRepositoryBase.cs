using ECommerce.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Core.DataAccess.EntityFramework
{
    public class EFEntityFrameworkRepositoryBase<TEntity, TContext>
        : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        private readonly TContext context;

        public EFEntityFrameworkRepositoryBase(TContext context)
        {
            this.context = context;
        }
        public async Task Add(TEntity entity)
        {
            var addedEntity = context.Entry(entity);
            addedEntity.State = EntityState.Added;
            await context.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            // Attach entity if not already tracked
            if (context.Entry(entity).State == EntityState.Detached)
            {
                context.Set<TEntity>().Attach(entity);
            }

            // Mark entity as deleted
            context.Entry(entity).State = EntityState.Deleted;

            // Save changes to the database
            await context.SaveChangesAsync();
        }


        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return await context.Set<TEntity>().SingleOrDefaultAsync(filter);
        }

        public async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ?
                await context.Set<TEntity>().ToListAsync() :
                await context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public async Task Update(TEntity entity)
        {
            var deletedEntity = context.Entry(entity);
            deletedEntity.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}

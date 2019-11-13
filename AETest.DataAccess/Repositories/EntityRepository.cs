using AETest.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AETest.DataAccess.Repositories
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        private readonly IDbContext _context;

        public virtual IQueryable<TEntity> Entities { get => _context.Set<TEntity>(); }

        public EntityRepository(IDbContext context)
        {
            _context = context;
        }

        public virtual Task Add(TEntity entity)
        {
            return UseAndSave(() => _context.Add(entity));
        }

        public virtual Task Update(TEntity entity)
        {
            return UseAndSave(() => _context.Update(entity));
        }

        public virtual Task Delete(TEntity entity)
        {
            return UseAndSave(() => _context.Delete(entity));
        }

        public virtual async Task<TEntity> FindEntity(Expression<Func<TEntity, bool>> criteria)
        {
            return await Entities.FirstOrDefaultAsync(criteria);
        }

        protected async Task UseAndSave(Func<Task> action)
        {
            await action.Invoke();
            await _context.SaveChanges();
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AETest.DataAccess
{
    public interface IEntityRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);

        Task<TEntity> FindEntity(Expression<Func<TEntity, bool>> ctiteria);
    }
}

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AETest.DataAccess
{
    /// <summary>
    /// Asyncronous repository to perform actions or query entities
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds and saves entity into database
        /// </summary>
        Task Add(TEntity entity);

        /// <summary>
        /// Updates and saves entity into database
        /// </summary>
        Task Update(TEntity entity);

        /// <summary>
        /// Perform remove of entity from database
        /// </summary>
        Task Delete(TEntity entity);

        /// <summary>
        /// Finds the entity by the given criteria
        /// </summary>
        /// <param name="ctiteria"></param>
        /// <returns></returns>
        Task<TEntity> FindEntity(Expression<Func<TEntity, bool>> ctiteria);
    }
}

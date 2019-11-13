using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace AETest.DataAccess
{
    public interface IDbContext
    {
        /// <summary>
        /// Async adds the entity to the underlying storage
        /// </summary>
        Task Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Async udpates the entity to the underlying storage
        /// </summary>
        Task Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Async deletes the entity to the underlying storage
        /// </summary>
        Task Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// A set to be used for accessing entities in the table
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Performs save of all pending entities
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> SaveChanges();
    }
}

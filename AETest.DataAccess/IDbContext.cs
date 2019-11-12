using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace AETest.DataAccess
{
    public interface IDbContext
    {
        Task Add<TEntity>(TEntity entity) where TEntity : class;
        Task Update<TEntity>(TEntity entity) where TEntity : class;
        Task Delete<TEntity>(TEntity entity) where TEntity : class;

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChanges();

        DatabaseFacade Database { get; }
    }
}

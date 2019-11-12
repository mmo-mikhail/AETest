using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AETest.DataAccess
{
    public class DatabaseContext : DbContext, IDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        Task IDbContext.Delete<TEntity>(TEntity entity)
        {
            return Task.Run(() => this.Remove(entity));
        }

        Task IDbContext.Add<TEntity>(TEntity entity)
        {
            return this.AddAsync(entity);
        }

        Task IDbContext.Update<TEntity>(TEntity entity)
        {
            return Task.Run(() => this.Update(entity));
        }

        Task<int> IDbContext.SaveChanges()
        {
            return this.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var applyGenericMethod = typeof(ModelBuilder).GetMethods()
                .Where(m => m.Name == "ApplyConfiguration"
                    && m.GetParameters().FirstOrDefault()?.ParameterType.GetGenericTypeDefinition()
                        == typeof(IEntityTypeConfiguration<>).GetGenericTypeDefinition())
                .FirstOrDefault();

            var configurators = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.BaseType != null && t.IsClass && !t.IsAbstract);
            foreach (var configType in configurators)
            {
                var intf = configType.GetInterfaces().FirstOrDefault(i =>
                    i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
                if (intf == null)
                    continue;
                var configuratorInstance = Activator.CreateInstance(configType);
                var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(intf.GenericTypeArguments[0]);
                applyConcreteMethod.Invoke(modelBuilder, new object[] { configuratorInstance });
            }
        }
    }
}

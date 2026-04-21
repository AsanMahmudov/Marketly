using Marketly.Core.Common;
using Marketly.Core.Interfaces;
using Marketly.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Marketly.Data.Common
{
    public abstract class BaseRepository : IRepository
    {
        protected readonly ApplicationDbContext context;

        protected BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        protected DbSet<T> DbSet<T>() where T : class
        {
            return context.Set<T>();
        }
        public async Task AddAsync<T>(T entity) where T : class
        {
            await DbSet<T>().AddAsync(entity);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>();
        }

        public void Delete<T>(T entity) where T : class
        {
            DbSet<T>().Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}

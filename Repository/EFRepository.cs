using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webapi.data;
using webapi.model;

namespace webapi.Repository
{
    public abstract class EFRepository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : ApplicationDbContext
    {
        private readonly TContext _dbContext;
        public EFRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await _dbContext.FindAsync<TEntity>(id);
            if( entity != null)
            {
                _dbContext.Remove<TEntity>(entity);
                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }

        public Task<TEntity> Delete(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TEntity> Get(int id)
        {
            var entity = await _dbContext.FindAsync<TEntity>(id);
            return entity;
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
             _dbContext.Update<TEntity>(entity);
             await _dbContext.SaveChangesAsync();
             return entity;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data
{
    /// <summary>
    /// <inheritdoc cref="IRepository{TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {

        private readonly GatewayDbContext _dbContext;

        public Repository(GatewayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get object <see cref="TEntity"/> its unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the entity</param>
        /// <returns><see cref="TEntity"/> entity, if the record does not exist, it returns null</returns>
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Get  <see cref="TEntity"/>  entities by query
        /// </summary>
        /// <param name="query">Predicate to be used for query</param>
        /// <returns>List of entities</returns>
        public async Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> query)
        {
            return await GetEntitiesByQuery(query).ToListAsync();
        }

        /// <summary>
        /// Get  a <see cref="TEntity"/>  entity by query
        /// </summary>
        /// <param name="query">Predicate to be used for query</param>
        /// <returns>The <see cref="TEntity"/>  entity</returns>
        public virtual Task<TEntity> GetSingleByQueryAsync(Expression<Func<TEntity, bool>> query = null)
        {
            return GetEntitiesByQuery(query).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Creates a <see cref="TEntity"/> entity 
        /// </summary>
        /// <param name="entity">entity <see cref="TEntity"/> to be created</param>
        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        ///  Updates a <see cref="TEntity"/> entity 
        /// </summary>
        /// <param name="entity">entity <see cref="TEntity"/> to be updated</param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<TEntity> GetEntitiesByQuery(Expression<Func<TEntity, bool>> query)
        {
            return _dbContext.Set<TEntity>().AsNoTracking().Where(query);
        }
    }
}

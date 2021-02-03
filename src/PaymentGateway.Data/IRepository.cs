using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PaymentGateway.Data
{
    /// <summary>
    /// Defines a Generic Repository 
    /// Only the  methods that would be needed for this test are defined
    /// </summary>
    /// <typeparam name="TEntity">Generic type</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> query = null);
        Task<TEntity> GetSingleByQueryAsync(Expression<Func<TEntity, bool>> query = null);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
    }

}
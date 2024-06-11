using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShippingBasketAPI.Data.Repository
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        public Task<T> GetTAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null);
        public Task AddAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}

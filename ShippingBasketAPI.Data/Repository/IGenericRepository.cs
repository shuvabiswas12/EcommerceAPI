using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShippingBasketAPI.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll(string? includeProperties = null, Expression<Func<T, bool>>? predicate = null);
        public Task<T> GetT(Expression<Func<T, bool>> predicate, string? includeProperties = null);
        public Task Add(T entity);
        public Task Delete(T entity);
        public Task DeleteRange(IEnumerable<T> entities);
    }
}

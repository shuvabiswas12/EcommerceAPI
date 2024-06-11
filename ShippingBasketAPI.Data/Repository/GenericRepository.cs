using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShippingBasketAPI.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public Task Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll(string? includeProperties = null, Expression<Func<T, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetT(Expression<Func<T, bool>> predicate, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }
    }
}

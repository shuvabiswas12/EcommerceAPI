using ShippingBasketAPI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingBasketAPI.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}

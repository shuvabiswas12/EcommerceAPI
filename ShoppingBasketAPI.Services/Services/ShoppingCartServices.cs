using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.Services
{
    public class ShoppingCartServices : IShoppingCartServices
    {
        private IUnitOfWork _unitOfWork;

        public ShoppingCartServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddProductToShoppingCart(ShoppingCartCreateRequestDTO shoppingCartCreateRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCartResponseDTO> GetShoppingCartByUserId()
        {
            throw new NotImplementedException();
        }

        public Task RemoveProductsFromShoppingCart(List<string> productId)
        {
            throw new NotImplementedException();
        }
    }
}

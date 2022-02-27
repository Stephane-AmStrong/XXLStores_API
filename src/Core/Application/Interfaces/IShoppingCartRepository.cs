using Application.Features.ShoppingCarts.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<PagedList<ShoppingCart>> GetPagedListAsync(GetShoppingCartsQuery getShoppingCartsQuery);

        Task<ShoppingCart> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(ShoppingCart shoppingCart);

        Task CreateAsync(ShoppingCart shoppingCart);
        Task UpdateAsync(ShoppingCart shoppingCart);
        Task UpdateAsync(IEnumerable<ShoppingCart> events);
        Task DeleteAsync(ShoppingCart shoppingCart);
    }
}

using Application.Features.ShoppingCarts.Commands.CreateShoppingCart;
using Application.Features.ShoppingCarts.Queries.GetShoppingCarts;
using Application.Parameters;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<PagedList<ShoppingCart>> GetPagedListAsync(GetShoppingCartsQuery eventsQuery);

        Task<ShoppingCart> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(ShoppingCart shoppingCart);

        Task CreateAsync(ShoppingCart shoppingCart);
        Task UpdateAsync(ShoppingCart shoppingCart);
        Task UpdateAsync(IEnumerable<ShoppingCart> events);
        Task DeleteAsync(ShoppingCart shoppingCart);
    }
}

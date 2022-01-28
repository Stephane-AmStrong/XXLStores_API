using Application.Features.ShoppingCartItems.Commands.CreateShoppingCartItem;
using Application.Features.ShoppingCartItems.Queries.GetShoppingCartItems;
using Application.Parameters;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShoppingCartItemRepository
    {
        Task<PagedList<ShoppingCartItem>> GetPagedListAsync(GetShoppingCartItemsQuery eventsQuery);

        Task<ShoppingCartItem> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(ShoppingCartItem shoppingCartItem);

        Task CreateAsync(ShoppingCartItem shoppingCartItem);
        Task UpdateAsync(ShoppingCartItem shoppingCartItem);
        Task UpdateAsync(IEnumerable<ShoppingCartItem> events);
        Task DeleteAsync(ShoppingCartItem shoppingCartItem);
    }
}

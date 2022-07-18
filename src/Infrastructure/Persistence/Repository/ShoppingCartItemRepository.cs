using Application.Features.ShoppingCartItems.Queries.GetPagedList;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ShoppingCartItemRepository : RepositoryBase<ShoppingCartItem>, IShoppingCartItemRepository
    {
        private ISortHelper<ShoppingCartItem> _sortHelper;

        public ShoppingCartItemRepository
        (
            ApplicationDbContext appDbContext,
            ISortHelper<ShoppingCartItem> sortHelper
        ) : base(appDbContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<ShoppingCartItem>> GetPagedListAsync(GetShoppingCartItemsQuery shoppingCartItemsQuery)
        {
            var shoppingCartItems = Enumerable.Empty<ShoppingCartItem>().AsQueryable();

            ApplyFilters(ref shoppingCartItems, shoppingCartItemsQuery);

            PerformSearch(ref shoppingCartItems, shoppingCartItemsQuery.SearchTerm);

            var sortedShoppingCartItems = _sortHelper.ApplySort(shoppingCartItems, shoppingCartItemsQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<ShoppingCartItem>.ToPagedList
                (
                    sortedShoppingCartItems,
                    shoppingCartItemsQuery.PageNumber,
                    shoppingCartItemsQuery.PageSize)
                );
        }


        public async Task<ShoppingCartItem> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(shoppingCartItem => shoppingCartItem.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistAsync(ShoppingCartItem shoppingCartItem)
        {
            return await BaseFindByCondition(x => x.ItemId == shoppingCartItem.ItemId && x.ShoppingCartId == shoppingCartItem.ShoppingCartId && x.UnitPrice == shoppingCartItem.UnitPrice)
                .AnyAsync();
        }

        public async Task CreateAsync(ShoppingCartItem shoppingCartItem)
        {
            await BaseCreateAsync(shoppingCartItem);
        }

        public async Task UpdateAsync(ShoppingCartItem shoppingCartItem)
        {
            await BaseUpdateAsync(shoppingCartItem);
        }

        public async Task UpdateAsync(IEnumerable<ShoppingCartItem> shoppingCartItems)
        {
            await BaseUpdateAsync(shoppingCartItems);
        }

        public async Task DeleteAsync(ShoppingCartItem shoppingCartItem)
        {
            await BaseDeleteAsync(shoppingCartItem);
        }

        private void ApplyFilters(ref IQueryable<ShoppingCartItem> shoppingCartItems, GetShoppingCartItemsQuery shoppingCartItemsQuery)
        {
            shoppingCartItems = BaseFindAll()
                .Include(x=> x.Item);

            /*
            if (shoppingCartItemsQuery.MinCreateAt != null)
            {
                shoppingCartItems = shoppingCartItems.Where(x => x.CreateAt >= shoppingCartItemsQuery.MinCreateAt);
            }

            if (shoppingCartItemsQuery.MaxCreateAt != null)
            {
                shoppingCartItems = shoppingCartItems.Where(x => x.CreateAt < shoppingCartItemsQuery.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<ShoppingCartItem> shoppingCartItems, string searchTerm)
        {
            if (!shoppingCartItems.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            shoppingCartItems = shoppingCartItems.Where(x => x.Item.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }


    }
}

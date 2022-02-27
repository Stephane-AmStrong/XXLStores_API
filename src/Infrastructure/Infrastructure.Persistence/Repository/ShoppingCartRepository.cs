using Application.Features.ShoppingCarts.Queries.GetPagedList;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCart>, IShoppingCartRepository
    {
        private ISortHelper<ShoppingCart> _sortHelper;

        public ShoppingCartRepository
        (
            RepositoryContext repositoryContext,
            ISortHelper<ShoppingCart> sortHelper
        ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<ShoppingCart>> GetPagedListAsync(GetShoppingCartsQuery shoppingCartsQuery)
        {
            var shoppingCarts = Enumerable.Empty<ShoppingCart>().AsQueryable();

            ApplyFilters(ref shoppingCarts, shoppingCartsQuery);

            PerformSearch(ref shoppingCarts, shoppingCartsQuery.SearchTerm);

            var sortedShoppingCarts = _sortHelper.ApplySort(shoppingCarts, shoppingCartsQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<ShoppingCart>.ToPagedList
                (
                    sortedShoppingCarts,
                    shoppingCartsQuery.PageNumber,
                    shoppingCartsQuery.PageSize)
                );
        }


        public async Task<ShoppingCart> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(shoppingCart => shoppingCart.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistAsync(ShoppingCart shoppingCart)
        {
            return await BaseFindByCondition(x => x.CustomerId == shoppingCart.CustomerId && x.OrderAt == shoppingCart.OrderAt && x.Total == shoppingCart.Total)
                .AnyAsync();
        }

        public async Task CreateAsync(ShoppingCart shoppingCart)
        {
            await BaseCreateAsync(shoppingCart);
        }

        public async Task UpdateAsync(ShoppingCart shoppingCart)
        {
            await BaseUpdateAsync(shoppingCart);
        }

        public async Task UpdateAsync(IEnumerable<ShoppingCart> shoppingCarts)
        {
            await BaseUpdateAsync(shoppingCarts);
        }

        public async Task DeleteAsync(ShoppingCart shoppingCart)
        {
            await BaseDeleteAsync(shoppingCart);
        }

        private void ApplyFilters(ref IQueryable<ShoppingCart> shoppingCarts, GetShoppingCartsQuery shoppingCartsQuery)
        {
            shoppingCarts = BaseFindAll()
                .Include(x=>x.Customer);

            /*
            if (shoppingCartsQuery.MinCreateAt != null)
            {
                shoppingCarts = shoppingCarts.Where(x => x.CreateAt >= shoppingCartsQuery.MinCreateAt);
            }

            if (shoppingCartsQuery.MaxCreateAt != null)
            {
                shoppingCarts = shoppingCarts.Where(x => x.CreateAt < shoppingCartsQuery.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<ShoppingCart> shoppingCarts, string searchTerm)
        {
            if (!shoppingCarts.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            shoppingCarts = shoppingCarts.Where(x => x.Customer.FirstName.ToLower().Contains(searchTerm.Trim().ToLower()) || x.Customer.LastName.ToLower().Contains(searchTerm.Trim().ToLower()));
        }


    }
}

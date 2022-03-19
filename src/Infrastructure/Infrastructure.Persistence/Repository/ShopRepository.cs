using Application.Features.Shops.Queries.GetPagedList;
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
    public class ShopRepository : RepositoryBase<Shop>, IShopRepository
    {
        private ISortHelper<Shop> _sortHelper;

        public ShopRepository
        (
            ApplicationDbContext appDbContext,
            ISortHelper<Shop> sortHelper
        ) : base(appDbContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Shop>> GetPagedListAsync(GetShopsQuery shopsQuery)
        {
            var shops = Enumerable.Empty<Shop>().AsQueryable();

            ApplyFilters(ref shops, shopsQuery);

            PerformSearch(ref shops, shopsQuery.SearchTerm);

            var sortedShops = _sortHelper.ApplySort(shops, shopsQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<Shop>.ToPagedList
                (
                    sortedShops,
                    shopsQuery.PageNumber,
                    shopsQuery.PageSize)
                );
        }


        public async Task<Shop> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(shop => shop.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistAsync(Shop shop)
        {
            return await BaseFindByCondition(x => x.Name == shop.Name && x.OwnerId == shop.OwnerId)
                .AnyAsync();
        }

        public async Task CreateAsync(Shop shop)
        {
            await BaseCreateAsync(shop);
        }

        public async Task UpdateAsync(Shop shop)
        {
            await BaseUpdateAsync(shop);
        }

        public async Task UpdateAsync(IEnumerable<Shop> shops)
        {
            await BaseUpdateAsync(shops);
        }

        public async Task DeleteAsync(Shop shop)
        {
            await BaseDeleteAsync(shop);
        }

        private void ApplyFilters(ref IQueryable<Shop> shops, GetShopsQuery shopsQuery)
        {
            shops = BaseFindAll();

            /*
            if (shopsQuery.MinCreateAt != null)
            {
                shops = shops.Where(x => x.CreateAt >= shopsQuery.MinCreateAt);
            }

            if (shopsQuery.MaxCreateAt != null)
            {
                shops = shops.Where(x => x.CreateAt < shopsQuery.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Shop> shops, string searchTerm)
        {
            if (!shops.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            shops = shops.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }


    }
}

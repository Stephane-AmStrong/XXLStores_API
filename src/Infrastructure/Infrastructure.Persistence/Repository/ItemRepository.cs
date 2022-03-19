using Application.Features.Items.Queries.GetPagedList;
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
    public class ItemRepository : RepositoryBase<Item>, IItemRepository
    {
        private ISortHelper<Item> _sortHelper;

        public ItemRepository
        (
            ApplicationDbContext appDbContext,
            ISortHelper<Item> sortHelper
        ) : base(appDbContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Item>> GetPagedListAsync(GetItemsQuery itemsQuery)
        {
            var items = Enumerable.Empty<Item>().AsQueryable();

            ApplyFilters(ref items, itemsQuery);

            PerformSearch(ref items, itemsQuery.SearchTerm);

            var sortedItems = _sortHelper.ApplySort(items, itemsQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<Item>.ToPagedList
                (
                    sortedItems,
                    itemsQuery.PageNumber,
                    itemsQuery.PageSize)
                );
        }


        public async Task<Item> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(item => item.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistAsync(Item item)
        {
            return await BaseFindByCondition(x => x.Name == item.Name && x.ShopId == item.ShopId)
                .AnyAsync();
        }

        public async Task CreateAsync(Item item)
        {
            await BaseCreateAsync(item);
        }

        public async Task UpdateAsync(Item item)
        {
            await BaseUpdateAsync(item);
        }

        public async Task UpdateAsync(IEnumerable<Item> items)
        {
            await BaseUpdateAsync(items);
        }

        public async Task DeleteAsync(Item item)
        {
            await BaseDeleteAsync(item);
        }

        private void ApplyFilters(ref IQueryable<Item> items, GetItemsQuery itemsQuery)
        {
            items = BaseFindAll();

            /*
            if (itemsQuery.MinCreateAt != null)
            {
                items = items.Where(x => x.CreateAt >= itemsQuery.MinCreateAt);
            }

            if (itemsQuery.MaxCreateAt != null)
            {
                items = items.Where(x => x.CreateAt < itemsQuery.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Item> items, string searchTerm)
        {
            if (!items.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            items = items.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }


    }
}

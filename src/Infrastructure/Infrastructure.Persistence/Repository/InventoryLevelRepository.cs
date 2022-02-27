using Application.Features.InventoryLevels.Queries.GetPagedList;
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
    public class InventoryLevelRepository : RepositoryBase<InventoryLevel>, IInventoryLevelRepository
    {
        private ISortHelper<InventoryLevel> _sortHelper;

        public InventoryLevelRepository
        (
            RepositoryContext repositoryContext,
            ISortHelper<InventoryLevel> sortHelper
        ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<InventoryLevel>> GetPagedListAsync(GetInventoryLevelsQuery inventoryLevelsQuery)
        {
            var inventoryLevels = Enumerable.Empty<InventoryLevel>().AsQueryable();

            ApplyFilters(ref inventoryLevels, inventoryLevelsQuery);

            PerformSearch(ref inventoryLevels, inventoryLevelsQuery.SearchTerm);

            var sortedInventoryLevels = _sortHelper.ApplySort(inventoryLevels, inventoryLevelsQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<InventoryLevel>.ToPagedList
                (
                    sortedInventoryLevels,
                    inventoryLevelsQuery.PageNumber,
                    inventoryLevelsQuery.PageSize)
                );
        }


        public async Task<InventoryLevel> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(inventoryLevel => inventoryLevel.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistAsync(InventoryLevel inventoryLevel)
        {
            return await BaseFindByCondition(x => x.InStock == inventoryLevel.InStock && x.StockAfter == inventoryLevel.StockAfter && x.ItemId == inventoryLevel.ItemId && x.UpdatedAt == inventoryLevel.UpdatedAt)
                .AnyAsync();
        }

        public async Task CreateAsync(InventoryLevel inventoryLevel)
        {
            await BaseCreateAsync(inventoryLevel);
        }

        public async Task UpdateAsync(InventoryLevel inventoryLevel)
        {
            await BaseUpdateAsync(inventoryLevel);
        }

        public async Task UpdateAsync(IEnumerable<InventoryLevel> inventoryLevels)
        {
            await BaseUpdateAsync(inventoryLevels);
        }

        public async Task DeleteAsync(InventoryLevel inventoryLevel)
        {
            await BaseDeleteAsync(inventoryLevel);
        }

        private void ApplyFilters(ref IQueryable<InventoryLevel> inventoryLevels, GetInventoryLevelsQuery inventoryLevelsQuery)
        {
            inventoryLevels = BaseFindAll()
                .Include(x => x.Item);

            /*
            if (inventoryLevelsQuery.MinCreateAt != null)
            {
                inventoryLevels = inventoryLevels.Where(x => x.CreateAt >= inventoryLevelsQuery.MinCreateAt);
            }

            if (inventoryLevelsQuery.MaxCreateAt != null)
            {
                inventoryLevels = inventoryLevels.Where(x => x.CreateAt < inventoryLevelsQuery.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<InventoryLevel> inventoryLevels, string searchTerm)
        {
            if (!inventoryLevels.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            inventoryLevels = inventoryLevels.Where(x => x.Item.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }


    }
}

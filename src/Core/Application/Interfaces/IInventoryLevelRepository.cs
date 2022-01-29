using Application.Features.InventoryLevels.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IInventoryLevelRepository
    {
        Task<PagedList<InventoryLevel>> GetPagedListAsync(GetInventoryLevelsQuery eventsQuery);

        Task<InventoryLevel> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(InventoryLevel inventoryLevel);

        Task CreateAsync(InventoryLevel inventoryLevel);
        Task UpdateAsync(InventoryLevel inventoryLevel);
        Task UpdateAsync(IEnumerable<InventoryLevel> events);
        Task DeleteAsync(InventoryLevel inventoryLevel);
    }
}

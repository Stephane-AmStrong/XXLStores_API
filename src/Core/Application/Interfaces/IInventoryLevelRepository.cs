using Application.Features.InventoryLevels.Commands.CreateInventoryLevel;
using Application.Features.InventoryLevels.Queries.GetInventoryLevels;
using Application.Parameters;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
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

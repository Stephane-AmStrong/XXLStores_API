using Application.Features.Items.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IItemRepository
    {
        Task<PagedList<Item>> GetPagedListAsync(GetItemsQuery eventsQuery);

        Task<Item> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(Item item);

        Task CreateAsync(Item item);
        Task UpdateAsync(Item item);
        Task UpdateAsync(IEnumerable<Item> events);
        Task DeleteAsync(Item item);
    }
}

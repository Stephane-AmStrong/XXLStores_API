using Application.Features.Shops.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShopRepository
    {
        Task<PagedList<Shop>> GetPagedListAsync(GetShopsQuery eventsQuery);

        Task<Shop> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(Shop shop);

        Task CreateAsync(Shop shop);
        Task UpdateAsync(Shop shop);
        Task UpdateAsync(IEnumerable<Shop> events);
        Task DeleteAsync(Shop shop);
    }
}

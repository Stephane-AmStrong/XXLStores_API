using Application.Features.AppUsers.Queries.GetPagedList;
using Application.Models;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppUserRepository
    {
        Task<PagedList<AppUser>> GetPagedListAsync(GetAppUsersQuery getAllAppUsersQuery);

        Task<int> CountUsersAsync();
        Task<AppUser> GetByIdAsync(string id);
        Task<bool> ExistAsync(AppUser appUser);

        Task CreateAsync(AppUser appUser);
        Task UpdateAsync(AppUser appUser);
        Task UpdateAsync(IEnumerable<AppUser> events);
        Task DeleteAsync(AppUser appUser);
    }
}

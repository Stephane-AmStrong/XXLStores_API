using Domain.Common;
using System;

namespace Application.Features.AppUsers.Queries.GetPagedList
{
    public record AppUsersViewModel : AuditableBaseEntity
    {
        public string ImgLink { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}

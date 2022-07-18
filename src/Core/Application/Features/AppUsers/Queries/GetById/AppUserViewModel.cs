using Domain.Common;
using System;

namespace Application.Features.AppUsers.Queries.GetById
{
    public record AppUserViewModel : AuditableBaseEntity
    {
        public string ImgLink { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}

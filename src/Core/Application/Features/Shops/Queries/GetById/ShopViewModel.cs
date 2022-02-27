using Application.Features.AppUsers.Queries.GetById;
using Application.Features.Items.Queries.GetPagedList;
using Domain.Common;
using System;

namespace Application.Features.Shops.Queries.GetById
{
    public record ShopViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid OwnerId { get; set; }

        public virtual AppUserViewModel Owner { get; set; }
        public virtual ItemsViewModel[] Items { get; set; }
    }
}

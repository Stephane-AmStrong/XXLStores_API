using Domain.Common;
using System;

namespace Application.Features.Shops.Queries.GetPagedList
{
    public record ShopsViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid OwnerId { get; set; }
    }
}

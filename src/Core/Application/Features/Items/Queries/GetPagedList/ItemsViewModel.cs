using Domain.Common;
using System;

namespace Application.Features.Items.Queries.GetPagedList
{
    public record ItemsViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ShopId { get; set; }
    }
}

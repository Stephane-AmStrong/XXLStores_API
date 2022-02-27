using Domain.Common;
using System;

namespace Application.Features.ShoppingCarts.Queries.GetPagedList
{
    public record ShoppingCartsViewModel : AuditableBaseEntity
    {
        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int DeliveryFees { get; set; }
        public Guid CustomerId { get; set; }
    }
}

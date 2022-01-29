using Domain.Common;
using System;

namespace Application.Features.ShoppingCartItems.Queries.GetPagedList
{
    public record ShoppingCartItemsViewModel : AuditableBaseEntity
    {
        public Guid ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public Guid ItemId { get; set; }
    }
}

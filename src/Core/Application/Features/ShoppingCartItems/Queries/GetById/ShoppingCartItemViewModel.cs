using Application.Features.Items.Queries.GetById;
using Application.Features.ShoppingCarts.Queries.GetById;
using Domain.Common;
using System;

namespace Application.Features.ShoppingCartItems.Queries.GetById
{
    public record ShoppingCartItemViewModel : AuditableBaseEntity
    {
        public Guid ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public Guid ItemId { get; set; }

        public virtual ShoppingCartViewModel ShoppingCart { get; set; }

        public virtual ItemViewModel Item { get; set; }
    }
}

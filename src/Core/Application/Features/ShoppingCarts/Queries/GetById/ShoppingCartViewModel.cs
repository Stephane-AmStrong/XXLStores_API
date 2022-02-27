using Application.Features.ShoppingCartItems.Queries.GetPagedList;
using Domain.Common;
using System;
using System.Collections.Generic;

namespace Application.Features.ShoppingCarts.Queries.GetById
{
    public record ShoppingCartViewModel : AuditableBaseEntity
    {
        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int DeliveryFees { get; set; }
        public Guid CustomerId { get; set; }

        public virtual ICollection<ShoppingCartItemsViewModel> ShoppingCartItems { get; set; }
    }
}

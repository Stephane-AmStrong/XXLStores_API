using Application.Features.ShoppingCarts.Queries.GetById;
using Domain.Common;
using System;

namespace Application.Features.Payments.Queries.GetById
{
    public record PaymentViewModel : AuditableBaseEntity
    {
        public int MoneyAmount { get; set; }
        public DateTime? PaidAt { get; set; }
        public Guid ShoppingCartId { get; set; }

        public virtual ShoppingCartViewModel ShoppingCart { get; set; }
    }
}

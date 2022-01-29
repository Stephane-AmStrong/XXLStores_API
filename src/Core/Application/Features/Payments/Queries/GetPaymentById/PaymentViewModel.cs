using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Features.InventoryLevels.Queries.GetInventoryLevels;
using Application.Features.ShoppingCartItems.Queries.GetShoppingCartItems;
using Application.Features.ShoppingCarts.Queries.GetShoppingCartById;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Payments.Queries.GetPaymentById
{
    public record PaymentViewModel : AuditableBaseEntity
    {
        public int MoneyAmount { get; set; }
        public DateTime? PaidAt { get; set; }
        public Guid ShoppingCartId { get; set; }

        public virtual ShoppingCartViewModel ShoppingCart { get; set; }
    }
}

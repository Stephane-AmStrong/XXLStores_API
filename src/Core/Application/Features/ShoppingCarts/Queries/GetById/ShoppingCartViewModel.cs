using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Features.InventoryLevels.Queries.GetInventoryLevels;
using Application.Features.ShoppingCartItems.Queries.GetShoppingCartItemById;
using Application.Features.ShoppingCartItems.Queries.GetShoppingCartItems;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

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

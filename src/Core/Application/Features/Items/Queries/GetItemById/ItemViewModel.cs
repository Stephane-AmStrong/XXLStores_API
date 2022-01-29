using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Features.InventoryLevels.Queries.GetInventoryLevels;
using Application.Features.ShoppingCartItems.Queries.GetShoppingCartItems;
using Application.Features.Shops.Queries.GetShopById;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Items.Queries.GetItemById
{
    public record ItemViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ShopId { get; set; }

        public virtual InventoryLevelsViewModel[] InventoryLevels { get; set; }
        public virtual ShoppingCartItemsViewModel[] ShoppingCartItems { get; set; }

        public virtual CategoryViewModel Category { get; set; }

        public virtual ShopViewModel Shop { get; set; }
    }
}

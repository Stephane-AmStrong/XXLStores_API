using Application.Features.Categories.Queries.GetById;
using Application.Features.InventoryLevels.Queries.GetPagedList;
using Application.Features.ShoppingCartItems.Queries.GetPagedList;
using Application.Features.Shops.Queries.GetById;
using Domain.Common;
using System;

namespace Application.Features.Items.Queries.GetById
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

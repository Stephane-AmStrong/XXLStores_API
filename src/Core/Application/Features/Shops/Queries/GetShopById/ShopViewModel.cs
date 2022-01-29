using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Features.InventoryLevels.Queries.GetInventoryLevels;
using Application.Features.Items.Queries.GetItems;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Shops.Queries.GetShopById
{
    public record ShopViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid OwnerId { get; set; }

        public virtual ItemsViewModel[] Items { get; set; }
    }
}

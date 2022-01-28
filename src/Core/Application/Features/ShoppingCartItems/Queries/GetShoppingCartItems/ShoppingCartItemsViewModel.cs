﻿using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Features.Categories.Queries.GetCategoryById;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ShoppingCartItems.Queries.GetShoppingCartItems
{
    public record ShoppingCartItemsViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ShopId { get; set; }
    }
}

using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Features.Categories.Queries.GetCategoryById;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ShoppingCarts.Queries.GetPagedList
{
    public record ShoppingCartsViewModel : AuditableBaseEntity
    {
        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int DeliveryFees { get; set; }
        public Guid CustomerId { get; set; }
    }
}

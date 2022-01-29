using Application.Features.Items.Queries.GetItems;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Categories.Queries.GetById
{
    public record CategoryViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ItemsViewModel[] Items { get; set; }
    }
}

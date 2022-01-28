using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Categories.Queries.GetCategories
{
    public record CategoriesViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

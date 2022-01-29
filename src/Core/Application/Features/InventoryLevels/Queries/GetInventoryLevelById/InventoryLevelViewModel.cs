using Application.Features.Items.Queries.GetItemById;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.InventoryLevels.Queries.GetInventoryLevelById
{
    public record InventoryLevelViewModel : AuditableBaseEntity
    {
        public int InStock { get; set; }
        public int StockAfter { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid ItemId { get; set; }

        public virtual ItemViewModel Item { get; set; }
    }
}
